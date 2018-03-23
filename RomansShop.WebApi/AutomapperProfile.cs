using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RomansShop.Domain.Entities;
using RomansShop.WebApi.ClientModels.Category;
using RomansShop.WebApi.ClientModels.Order;
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

            CreateMap<Order, OrderResponseModel>()
                .ForMember(
                resp => resp.Products,
                m => m.MapFrom(order => order.OrderProducts.Select(op => op.Product)));
                
            CreateMap<OrderRequestModel, Order>()
                .ForMember(
                order => order.OrderProducts,
                m => m.ResolveUsing<OrderRequestToOrderResolver>());
        }
    }

    public class OrderRequestToOrderResolver : IValueResolver<OrderRequestModel, Order, IList<OrderProduct>>
    {
        public IList<OrderProduct> Resolve(OrderRequestModel source, Order destination, IList<OrderProduct> member, ResolutionContext context)
        {
            source.Products
                .ToList()
                .ForEach(product => 
                {
                    member.Add(new OrderProduct { ProductId = product.Id, OrderId = destination.Id });
                });

            return member;
        }
    }
}