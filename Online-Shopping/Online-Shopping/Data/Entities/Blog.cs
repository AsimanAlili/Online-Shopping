using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Blog:BaseEntity
    {
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public List<BlogTag> BlogTags { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
