using System.ComponentModel.DataAnnotations;

using HeadphoneStore.Infrastructure.DependencyInjection.Options;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace HeadphoneStore.API.Filters;

public class FileValidationFilter : ActionFilterAttribute
{
    private readonly HashSet<string> _allowedExtensions;
    private readonly long _maxSize;
    private int _maxFiles;
    private readonly long _minSize;

    public FileValidationFilter(long maxSize, int maxFiles = 5, long minSize = 1024)
    {
        _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        _maxSize = maxSize;
        _minSize = minSize;
        _maxFiles = maxFiles;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var files = GetFilesFromContext(context);

            if (files != null)
            {
                await ValidateFilesAsync(context, files);
            }

            await next();
        }
        catch (ValidationException ex)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = "Validation Error",
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            context.Result = new StatusCodeResult(500);
        }
    }

    private IEnumerable<IFormFile> GetFilesFromContext(ActionExecutingContext context)
    {
        var files = new List<IFormFile>();

        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is IFormFile file)
            {
                files.Add(file);
            }
            else if (arg is IEnumerable<IFormFile> fileList)
            {
                files.AddRange(fileList);
            }
        }

        return files;
    }

    private async Task ValidateFilesAsync(ActionExecutingContext context, IEnumerable<IFormFile> files)
    {
        InitializeAllowedExtensions(context);

        var filesList = files.ToList();

        if (filesList.Count > _maxFiles)
        {
            throw new ValidationException($"Total number of files exceeds the maximum limit of {_maxFiles}.");
        }

        foreach (var file in filesList)
        {
            await ValidateFileAsync(file);
        }
    }

    private async Task<bool> ValidateFileAsync(IFormFile file)
    {
        if (file is null)
        {
            throw new ValidationException("File is null.");
        }

        if (string.IsNullOrWhiteSpace(file.FileName))
        {
            throw new ValidationException("File name is required.");
        }

        if (file.Length < _minSize)
        {
            throw new ValidationException($"File size is too small. Minimum size is {FormatFileSize(_minSize)}");
        }

        if (file.Length > _maxSize)
        {
            throw new ValidationException($"File size exceeds the maximum allowed size of {FormatFileSize(_maxSize)}");
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(ext))
        {
            var allowedExtList = string.Join(", ", _allowedExtensions.Select(x => x.TrimStart('.')).ToArray());
            throw new ValidationException($"Invalid file type. Allowed extensions: {allowedExtList}.");
        }

        return true;
    }

    private void InitializeAllowedExtensions(ActionExecutingContext context)
    {
        var mediaSettings = context.HttpContext.RequestServices.GetService<IOptions<MediaOption>>()?.Value;

        if (mediaSettings is null)
        {
            throw new ValidationException("Media settings is missing.");
        }

        if (mediaSettings.AllowFileTypes is null)
        {
            throw new ValidationException("Media settings are not properly configured.");
        }

        _allowedExtensions.Clear();

        foreach (var ext in mediaSettings.AllowFileTypes.Split(",", StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmedExt = ext.Trim().ToLowerInvariant();

            if (!trimmedExt.StartsWith("."))
            {
                trimmedExt = "." + trimmedExt;
            }

            _allowedExtensions.Add(trimmedExt);
        }

        if (mediaSettings.MaxFiles is not null)
        {
            _maxFiles = (int)mediaSettings.MaxFiles;
        }
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }
}
