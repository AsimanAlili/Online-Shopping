using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Discount:BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SaleTitle { get; set; }
        public string Photo { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime DiscountTime { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

    }
}
