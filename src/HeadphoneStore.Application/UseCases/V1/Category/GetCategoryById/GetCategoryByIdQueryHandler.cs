using AutoMapper;

using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

using Category = Domain.Aggregates.Categories.Entities.Category;
using Exceptions = Domain.Exceptions.Exceptions;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            throw new Exceptions.Category.NotFound();
        }

        if (category.IsDeleted)
        {
            throw new Exceptions.Category.AlreadyDeleted();
        }

        CategoryDtoBase? parent = null;

        if (category.ParentId is not null)
        {
            parent = _mapper.Map<CategoryDtoBase>(category.Parent);
        }

        List<CategoryDtoBase>? children = null;

        if (category.Children.Any())
        {
            children = _mapper.Map<List<CategoryDtoBase>>(category.Children);
        }

        var result = _mapper.Map<CategoryDto>(category);
        result.Parent = parent;
        result.Children = children;

        return Result.Success(result);
    }
}
