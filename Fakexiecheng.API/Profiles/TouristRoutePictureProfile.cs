using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Moldes;

namespace Fakexiecheng.API.Profiles
{
    public class TouristRoutePictureProfile:Profile
    {
        public TouristRoutePictureProfile() 
        {
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();

            CreateMap<TouristRoutePictureForCreationDto,TouristRoutePicture>();

            CreateMap<TouristRoutePicture, TouristRoutePictureForCreationDto>();
        }
    }
}
