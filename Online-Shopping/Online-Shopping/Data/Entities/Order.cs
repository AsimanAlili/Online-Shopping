using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Order:BaseEntity
    {
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, double.MaxValue)]
        public double DiscountedPrice { get; set; }

        public OrderStatus Status { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}
