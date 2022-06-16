﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fakexiecheng.API.Moldes
{
    public class ShoppingCart
    {
        [Key]

        public Guid Id { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<LineItem> ShoppingCartItems { get; set; }


    }
}
