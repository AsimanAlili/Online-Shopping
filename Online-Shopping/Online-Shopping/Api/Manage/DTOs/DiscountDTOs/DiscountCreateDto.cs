using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class DiscountCreateDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SaleTitle { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime DiscountTime { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
    public class DiscountCreateDtoValidator : AbstractValidator<DiscountCreateDto>
    {
        public DiscountCreateDtoValidator()
        {
            RuleFor(x => x.Title).MaximumLength(150).WithMessage("Length cannot be greater than 150!")
                .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.SubTitle).MaximumLength(150).WithMessage("Length cannot be greater than 150!");
            RuleFor(x => x.SaleTitle).MaximumLength(150).WithMessage("Length cannot be greater than 150!");
            RuleFor(x => x.RedirectUrl).MaximumLength(100).WithMessage("Length cannot be greater than 100!");
        }
    }
}
