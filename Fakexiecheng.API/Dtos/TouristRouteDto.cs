﻿using Fakexiecheng.API.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fakexiecheng.API.Dtos
{
    public class TouristRouteDto
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }

        public string Fees { get; set; }

        public string Notes { get; set; }


        public double? Rating { get; set; }
        public string TravalDays { get; set; }

        public string TripType { get; set; }

        public string DepartureCity { get; set; }

        public ICollection<TouristRoutePictureDto> TouristRoutePictures { get; set; }


        public ICollection<Test> Test { get; set; } = new List<Test>{ };
        
    }
}
