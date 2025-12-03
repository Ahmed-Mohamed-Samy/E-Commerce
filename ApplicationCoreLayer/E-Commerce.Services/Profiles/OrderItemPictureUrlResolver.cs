using AutoMapper;
using AutoMapper.Execution;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Services.Profiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.ProductUrl))
                return string.Empty;

            if (source.Product.ProductUrl.StartsWith("http"))
                return source.Product.ProductUrl;

            var BaseUrl = _configuration.GetSection("URLs")["BaseUrl"];

            if (string.IsNullOrEmpty(BaseUrl)) return string.Empty;


            var picUrl = $"{BaseUrl}{source.Product.ProductUrl}";

            return picUrl;
        }
    }
}