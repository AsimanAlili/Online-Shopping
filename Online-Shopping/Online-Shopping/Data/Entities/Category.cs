using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
    }
}
