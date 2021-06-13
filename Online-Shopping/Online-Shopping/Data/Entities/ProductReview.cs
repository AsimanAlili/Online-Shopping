using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class ProductReview:BaseEntity
    {
        public int ProductId { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Message { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }

        public Product Product { get; set; }
    }
}
