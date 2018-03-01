using AutoMapper;
using RomansShop.Domain;

namespace RomansShop.WebApi
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<EditProductRequest, Product>();

            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<EditCategoryRequest, Category>();
        }
    }
}