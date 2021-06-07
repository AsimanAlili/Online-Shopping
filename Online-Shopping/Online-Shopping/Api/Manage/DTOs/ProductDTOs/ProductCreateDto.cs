using FluentValidation;
using Microsoft.AspNetCore.Http;
using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ProductCreateDto
    {
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
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
        public List<ProductColorDto> ProductColors { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; }
        public List<ProductPhotoDto> ProductPhotos { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class ProductColorDto
    {
        public int ColorId { get; set; }
        public bool IsAvailableColor { get; set; }

    }
    public class ProductSizeDto
    {
        public int SizeId { get; set; }
        public bool IsAvailableSize { get; set; }

    }
    public class ProductPhotoDto
    {
        public string Name { get; set; }
        public int Order { get; set; }

    }

    #region ProductCreateValidator
    public class ProductCreateValidatorDto : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidatorDto()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Length cannot be greater than 50!")
               .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Slug).MaximumLength(200).WithMessage("Length cannot be greater than 200!")
                .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Desc).MaximumLength(2000).WithMessage("Length cannot be greater than 2000!");
            RuleFor(x => x.Specification).MaximumLength(2000).WithMessage("Length cannot be greater than 2000!");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price value cannot be less than 0!");
            RuleFor(x => x.ProducingPrice).GreaterThanOrEqualTo(0).WithMessage("Price value cannot be less than 0!");
            RuleFor(x => x.DiscountedPrice).GreaterThanOrEqualTo(0).WithMessage("Price value cannot be less than 0!");
            RuleFor(x => x.DiscountPercent).GreaterThanOrEqualTo(0).WithMessage("Price value cannot be less than 0!");
            RuleFor(x => x.BrandId).NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.SubCategoryId).NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Gender).NotEmpty().NotNull().WithMessage("Cannot be empty");

        }
    }
    #endregion

}
