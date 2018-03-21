using AutoMapper;
using RomansShop.Domain.Entities;
using RomansShop.WebApi.ClientModels.Category;
using RomansShop.WebApi.ClientModels.Product;
using RomansShop.WebApi.ClientModels.User;

namespace RomansShop.WebApi
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductResponseModel>();
            CreateMap<ProductRequestModel, Product>();

            CreateMap<Category, CategoryResponseModel>();
            CreateMap<CategoryRequestModel, Category>();

            CreateMap<User, UserResponseModel>();
            CreateMap<AddUserRequestModel, User>();
            CreateMap<UpdateUserRequestModel, User>();
        }
    }
}