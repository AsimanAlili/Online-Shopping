using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SliderCreateDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string RedirectUrl { get; set; }
        public string Photo { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
    public class SliderCreateDtoValidator : AbstractValidator<SliderCreateDto>
    {
        public SliderCreateDtoValidator()
        {
            RuleFor(x => x.Title).MaximumLength(150).WithMessage("Length cannot be greater than 50!")
                .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Order).GreaterThan(0).WithMessage("Order value cannot be less than 1!");
            RuleFor(x => x.Text).MaximumLength(350).WithMessage("Length cannot be greater than 100!");
            RuleFor(x => x.Photo).MaximumLength(100).WithMessage("Length cannot be greater than 100!");
            RuleFor(x => x.RedirectUrl).MaximumLength(350).WithMessage("Length cannot be greater than 1500!");

        }
    }
}
