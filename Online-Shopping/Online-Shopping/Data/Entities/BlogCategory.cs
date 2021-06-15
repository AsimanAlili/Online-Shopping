using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class BlogCategory:BaseEntity
    {
        public string Name { get; set; }
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Order { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
