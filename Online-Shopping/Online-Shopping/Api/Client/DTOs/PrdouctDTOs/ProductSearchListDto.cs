using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class ProductSearchListDto
    {
        public List<ProductSearchItemDto> Products { get; set; }
        public int TotalCount { get; set; }

    }

    public class ProductSearchItemDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string SubCategoryName { get; set; }

    }
}
