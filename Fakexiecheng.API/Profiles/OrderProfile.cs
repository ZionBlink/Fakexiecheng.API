using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Moldes;

namespace Fakexiecheng.API.Profiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {

            CreateMap<Order, OrderDto>()
                .ForMember(
                dest => dest.State,
                opt => { opt.MapFrom(src => src.State.ToString()); }
                );


        }

    }
}
