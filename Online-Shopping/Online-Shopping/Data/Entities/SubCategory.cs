using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
