using Fakexiecheng.API.validationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fakexiecheng.API.Dtos
{
    //数据验证特性
    [TouristRouteTitleMustBeDifferentFromDescriptionAttribute]
    //父类资源用于继承
    public abstract class TouristRouteForManipulationDto

    {
        [Required(ErrorMessage = "Title不可为空")]
        [MaxLength(100)]
        public string Title { get; set; }

        //虚函数用于继承
        [Required]
        [MaxLength(1500)]
        public virtual string Description { get; set; }

        public decimal Price { get; set; }
        // public decimal OriginalPrice { get; set; }

        //  public double? DiscountPresent { get; set; }
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

        //多看看
        //这里可以创建一个曾改图片资源的父类
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureForCreationDto>();
    }
}
