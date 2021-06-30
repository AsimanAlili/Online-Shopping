using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class DiscountGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SaleTitle { get; set; }
        public string Photo { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime DiscountTime { get; set; }
       
    }
}
