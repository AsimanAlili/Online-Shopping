using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BlogListDto
    {
        public List<BlogItemDto> Blogs { get; set; }
        public int TotalCount { get; set; }
    }
    public class BlogItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }
        public string BlogCategoryName { get; set; }
        public List<TagInBlogDto> BlogTags { get; set; }

    }
}
