using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class ProductPhoto:BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Order { get; set; }

        public Product Product { get; set; }
    }
}
