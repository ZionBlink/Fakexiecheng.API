using AutoMapper;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Models;
using Fakexiecheng.API.Moldes;
using System;

namespace Fakexiecheng.API.Profiles
{
    public class TouristRouteProfile : Profile
    {
        public TouristRouteProfile()
        {
            //把前面的数据，塑形成后面的模型进行输出
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1))
                )
                .ForMember(
                dest => dest.TravalDays,
                opt => opt.MapFrom(src => src.TravelDays.ToString())
                )
                .ForMember(
                dest => dest.TripType,
                opt => opt.MapFrom(src => src.TripType.ToString())
                )
                .ForMember(
                dest => dest.DepartureCity,
                opt => opt.MapFrom(src => src.DepartureCity.ToString())
                )                             
                ;
            

           CreateMap<TouristRouteForCreationDto, TouristRoute>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())

                );
            CreateMap<TouristRouteForUpdateDto, TouristRoute>();
            CreateMap<TouristRoute, TouristRouteForUpdateDto>();
            CreateMap<TouristRoute, TouristRouteSimplifyDto>()
                .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1))
                );
        }
    }
}
