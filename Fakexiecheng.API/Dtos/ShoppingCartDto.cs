using Fakexiecheng.API.Moldes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fakexiecheng.API.Dtos
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

       // public ApplicationUser User { get; set; }

        public ICollection<LineItem> ShoppingCartItems { get; set; }


    }
}
