using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class OrderGetDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
        public OrderStatus Status { get; set; }
        public ProductInOrderDto Product { get; set; }
        public AppUserInOrderDto AppUser { get; set; }
    }
    public class ProductInOrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class AppUserInOrderDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }

    }
}
