using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;
using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;
using HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;
using HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;
using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;
using HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;
using HeadphoneStore.Contract.Dtos.Identity.Role;
using HeadphoneStore.Contract.Services.Brand.BulkDelete;
using HeadphoneStore.Contract.Services.Brand.Create;
using HeadphoneStore.Contract.Services.Brand.Delete;
using HeadphoneStore.Contract.Services.Brand.GetAll;
using HeadphoneStore.Contract.Services.Brand.GetAllPaged;
using HeadphoneStore.Contract.Services.Brand.GetById;
using HeadphoneStore.Contract.Services.Brand.Update;
using HeadphoneStore.Contract.Services.Category.Create;
using HeadphoneStore.Contract.Services.Category.Delete;
using HeadphoneStore.Contract.Services.Category.GetCategoryById;
using HeadphoneStore.Contract.Services.Category.Update;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Contract.Services.Identity.Register;
using HeadphoneStore.Contract.Services.Product.Create;
using HeadphoneStore.Contract.Services.Product.Delete;
using HeadphoneStore.Contract.Services.Product.Update;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

namespace HeadphoneStore.Application.Mapper;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        // Authentication.
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterRequestDto, RegisterCommand>();

        // Role
        CreateMap<AppRole, RoleDto>();

        // Category
        CreateMap<CreateCategoryRequestDto, CreateCategoryCommand>();
        CreateMap<UpdateCategoryRequestDto, UpdateCategoryCommand>();
        CreateMap<DeleteCategoryRequestDto, DeleteCategoryCommand>();
        CreateMap<GetCategoryByIdRequestDto, GetCategoryByIdQuery>();

        // Brand
        CreateMap<CreateBrandRequestDto, CreateBrandCommand>();
        CreateMap<UpdateBrandRequestDto, UpdateBrandCommand>();
        CreateMap<DeleteBrandRequestDto, DeleteBrandCommand>();
        CreateMap<BulkDeleteBrandRequestDto, BulkDeleteBrandCommand>();
        CreateMap<GetBrandByIdRequestDto, GetBrandByIdQuery>();
        CreateMap<GetAllBrandsRequestDto, GetAllBrandsQuery>();
        CreateMap<GetAllBrandsPagedRequestDto, GetAllBrandsPagedQuery>();

        // Product
        CreateMap<CreateProductRequestDto, CreateProductCommand>();
        CreateMap<UpdateProductRequestDto, UpdateProductCommand>();
        CreateMap<DeleteProductRequestDto, DeleteProductCommand>();
    }
}
