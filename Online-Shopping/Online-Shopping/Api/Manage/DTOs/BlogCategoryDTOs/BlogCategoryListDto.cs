using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BlogCategoryListDto
    {
        public List<BlogCategoryItemDto> BlogCategories { get; set; }
        public int TotalCount { get; set; }

    }
    public class BlogCategoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

    }
}
