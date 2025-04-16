using HeadphoneStore.Contract.Dtos.Media;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.Abstracts.Interface.Services.Media;

public interface ICloudinaryService
{
    Task SaveFilesAsync(List<IFormFile> files, string type);

    Task RemoveFiles(List<string> paths);

    Task<(Stream FileStream, string ContentType, string FileName)> DownloadFiles(List<string> path);

    Task<List<FileDto>> UploadFilesToCloudinary(List<IFormFile> files, FileRequiredParamsDto dto);

    Task RemoveFilesFromCloudinary(List<DeleteFileDto> files);

    string? GenerateDownloadUrl(List<string> publicIds);
}
