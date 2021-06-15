using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<BlogTag> BlogTags { get; set; }

    }
}
