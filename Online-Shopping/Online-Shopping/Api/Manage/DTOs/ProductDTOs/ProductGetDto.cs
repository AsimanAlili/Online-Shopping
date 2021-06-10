using Online_Shopping.Data.Entities;
using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ProductGetDto
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
        public ProductInSubCategoryDto SubCategory { get; set; }
        public ProductInBrandDto Brand { get; set; }
        public List<ColorInProductDto> ProductColors { get; set; }
        public List<SizeInProductDto> ProductSizes { get; set; }
        public List<PhotoInProductDto> ProductPhotos { get; set; }


    }
    public class ProductInSubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductInBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ColorInProductDto
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public bool IsAvailableColor { get; set; }

    }
    public class SizeInProductDto
    {
        public int Id { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public bool IsAvailableSize { get; set; }

    }
    public class PhotoInProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

    }
}
