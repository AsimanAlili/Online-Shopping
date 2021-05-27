using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SubCategoryListDto
    {
        public List<SubCategoryItemDto> SubCategories { get; set; }
        public int TotalCount { get; set; }

    }
    public class SubCategoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string CategoryName { get; set; }
    }
}
