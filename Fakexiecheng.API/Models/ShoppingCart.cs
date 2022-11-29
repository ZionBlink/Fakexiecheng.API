using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fakexiecheng.API.Moldes
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 与用户之间的外键关系
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// user表
        /// </summary>
        public ApplicationUser User { get; set; }
        /// <summary>
        /// 购物车
        /// </summary>
        public ICollection<LineItem> ShoppingCartItems { get; set; }


    }
}
