using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class ProductListFilterDto
    {
        public List<ProductFilterItemDto> Products { get; set; }
        public int TotalCount { get; set; }
    }
    public class ProductFilterItemDto
    {
        public int? Id { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
