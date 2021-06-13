using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ProductListDto
    {
        public List<ProductItemDto> Products { get; set; }
        public int TotalCount { get; set; }

    }
    public class ProductItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Specification { get; set; }
        public string Slug { get; set; }
        public double Price { get; set; }
        public double ProducingPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public double DiscountPercent { get; set; }
        public bool IsNew { get; set; }
        public bool IsHotTrend { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsFeature { get; set; }
        public Gender Gender { get; set; }
        public string SubCategoryName { get; set; }
        public string BrandName { get; set; }

        public List<ColorInProductDto> ProductColors { get; set; }
        public List<SizeInProductDto> ProductSizes { get; set; }
        public List<PhotoInProductDto> ProductPhotos { get; set; }

    }
}
