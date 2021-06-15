using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class BlogTag:BaseEntity
    {

        public int TagId { get; set; }
        public int BlogId { get; set; }
        public Tag Tag { get; set; }
        public Blog Blog { get; set; }
    }
}
