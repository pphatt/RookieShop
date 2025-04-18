
using AutoMapper;

using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Abstracts.Repositories;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryDtoBase>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryDtoBase>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.FindAll(x => x.ParentId == null);

        var result = _mapper.Map<List<CategoryDtoBase>>(categories);

        return Result.Success(result);
    }
}
