using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO,OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(Dest => Dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(Dest => Dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(Dest => Dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
