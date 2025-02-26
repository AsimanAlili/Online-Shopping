﻿using Microsoft.AspNetCore.Http;
using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Product:BaseEntity
    {
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Specification { get; set; }
        public string Slug { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, double.MaxValue)]
        public double ProducingPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double DiscountedPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double DiscountPercent { get; set; }

        [Range(0, double.MaxValue)]
        public double Rate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsNew { get; set; }
        public bool IsHotTrend { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsFeature{ get; set; }
        public bool IsBookmarked { get; set; }
        public Gender Gender { get; set; }
        public Brand Brand { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
        public List<Order> Orders { get; set; }
        public List<ProductReview> ProductReviews { get; set; }

        [NotMapped]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<int> FileIds { get; set; }

    }
}
