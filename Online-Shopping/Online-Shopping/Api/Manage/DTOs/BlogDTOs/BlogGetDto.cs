using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BlogGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class BlogInBlogCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TagInBlogDto
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
