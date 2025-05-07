using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;
using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;
using HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;
using HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;
using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;
using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;
using HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;
using HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;
using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;
using HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;
using HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;
using HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;
using HeadphoneStore.Application.UseCases.V1.Product.GetProductById;
using HeadphoneStore.Application.UseCases.V1.Product.GetProductBySlug;
using HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Order.Enumerations;
using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Dtos.Identity.Role;
using HeadphoneStore.Shared.Services.Brand.ActiveBrand;
using HeadphoneStore.Shared.Services.Brand.BulkDelete;
using HeadphoneStore.Shared.Services.Brand.Create;
using HeadphoneStore.Shared.Services.Brand.Delete;
using HeadphoneStore.Shared.Services.Brand.GetAll;
using HeadphoneStore.Shared.Services.Brand.GetAllBrandsByProductProperties;
using HeadphoneStore.Shared.Services.Brand.GetAllPaged;
using HeadphoneStore.Shared.Services.Brand.GetById;
using HeadphoneStore.Shared.Services.Brand.InactiveBrand;
using HeadphoneStore.Shared.Services.Brand.Update;
using HeadphoneStore.Shared.Services.Category.ActivateCategory;
using HeadphoneStore.Shared.Services.Category.Create;
using HeadphoneStore.Shared.Services.Category.Delete;
using HeadphoneStore.Shared.Services.Category.GetAllCategories;
using HeadphoneStore.Shared.Services.Category.GetAllPaged;
using HeadphoneStore.Shared.Services.Category.GetCategoryById;
using HeadphoneStore.Shared.Services.Category.InactivateCategory;
using HeadphoneStore.Shared.Services.Category.Update;
using HeadphoneStore.Shared.Services.Identity.CreateUser;
using HeadphoneStore.Shared.Services.Identity.DeleteUser;
using HeadphoneStore.Shared.Services.Identity.GetAllUserPaged;
using HeadphoneStore.Shared.Services.Identity.GetUserById;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.Shared.Services.Identity.RefreshToken;
using HeadphoneStore.Shared.Services.Identity.Register;
using HeadphoneStore.Shared.Services.Identity.UpdateUser;
using HeadphoneStore.Shared.Services.Order.CreateOrder;
using HeadphoneStore.Shared.Services.Product.ActivateProduct;
using HeadphoneStore.Shared.Services.Product.Create;
using HeadphoneStore.Shared.Services.Product.Delete;
using HeadphoneStore.Shared.Services.Product.GetAllPaged;
using HeadphoneStore.Shared.Services.Product.GetById;
using HeadphoneStore.Shared.Services.Product.GetProductBySlug;
using HeadphoneStore.Shared.Services.Product.InactivateProduct;
using HeadphoneStore.Shared.Services.Product.Update;

namespace HeadphoneStore.Application.Mapper;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        // Authentication.
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterRequestDto, RegisterCommand>();
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();

        // User
        CreateMap<CreateUserRequestDto, CreateUserCommand>();
        CreateMap<UpdateUserRequestDto, UpdateUserCommand>();
        CreateMap<DeleteUserRequestDto, DeleteUserCommand>();
        CreateMap<GetUserByIdRequestDto, GetUserByIdQuery>();
        CreateMap<GetAllUserPagedRequestDto, GetAllUserPagedQuery>();

        // Role
        CreateMap<AppRole, RoleDto>();

        // Category
        CreateMap<CreateCategoryRequestDto, CreateCategoryCommand>();
        CreateMap<UpdateCategoryRequestDto, UpdateCategoryCommand>();
        CreateMap<DeleteCategoryRequestDto, DeleteCategoryCommand>();
        CreateMap<GetCategoryByIdRequestDto, GetCategoryByIdQuery>();
        CreateMap<GetAllCategoriesPagedRequestDto, GetAllCategoriesPagedQuery>();
        CreateMap<GetAllCategoriesRequestDto, GetAllCategoriesQuery>();
        CreateMap<ActivateCategoryRequestDto, ActivateCategoryCommand>();
        CreateMap<InactivateCategoryRequestDto, InactivateCategoryCommand>();

        // Brand
        CreateMap<CreateBrandRequestDto, CreateBrandCommand>();
        CreateMap<UpdateBrandRequestDto, UpdateBrandCommand>();
        CreateMap<DeleteBrandRequestDto, DeleteBrandCommand>();
        CreateMap<BulkDeleteBrandRequestDto, BulkDeleteBrandCommand>();
        CreateMap<GetBrandByIdRequestDto, GetBrandByIdQuery>();
        CreateMap<GetAllBrandsRequestDto, GetAllBrandsQuery>();
        CreateMap<GetAllBrandsPagedRequestDto, GetAllBrandsPagedQuery>();
        CreateMap<ActivateBrandRequestDto, ActivateBrandCommand>();
        CreateMap<InactivateBrandRequestDto, InactivateBrandCommand>();
        CreateMap<GetAllBrandsByProductPropertiesRequestDto, GetAllBrandsByProductPropertiesQuery>();

        // Product
        CreateMap<CreateProductRequestDto, CreateProductCommand>();
        CreateMap<UpdateProductRequestDto, UpdateProductCommand>();
        CreateMap<DeleteProductRequestDto, DeleteProductCommand>();
        CreateMap<GetProductByIdRequestDto, GetProductByIdQuery>();
        CreateMap<GetAllProductPagedRequestDto, GetAllProductsPagedQuery>();
        CreateMap<ActivateProductRequestDto, ActivateProductCommand>();
        CreateMap<InactivateProductRequestDto, InactivateProductCommand>();
        CreateMap<GetProductBySlugRequestDto, GetProductBySlugQuery>();

        // Order
        CreateMap<CreateOrderRequestDto, CreateOrderCommand>()
            .ForMember(dest => dest.ShippingAddress,
                        opt => opt.MapFrom(src => new ShippingAddress
                        {
                            StreetAddress = src.ShippingAddress.StreetAddress,
                            Ward = src.ShippingAddress.Ward,
                            District = src.ShippingAddress.District,
                            CityProvince = src.ShippingAddress.CityProvince
                        }))
            .ForMember(dest => dest.PaymentMethod,
                        opt => opt.MapFrom(src => Enum.Parse<PaymentMethod>(src.PaymentMethod)));
    }
}
