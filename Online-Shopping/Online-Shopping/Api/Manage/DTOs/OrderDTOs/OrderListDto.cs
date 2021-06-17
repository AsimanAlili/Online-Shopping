using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class OrderListDto
    {
        public List<OrderItemDto> Orders { get; set; }
        public int TotalCount { get; set; }
    }
    public class OrderItemDto
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string ProductName { get; set; }
        public string AppUserFullName { get; set; }
    }
}
