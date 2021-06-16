using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class FeatureCreateDto
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int Order { get; set; }

    }

    public  class FeatureCreateDtoValidator : AbstractValidator<FeatureCreateDto>
    {
        public FeatureCreateDtoValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100).WithMessage("Length cannot be greater than 100!")
              .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Icon).MaximumLength(200).WithMessage("Length cannot be greater than 200!");
            RuleFor(x => x.SubTitle).MaximumLength(200).WithMessage("Length cannot be greater than 200!");
            RuleFor(x => x.Order).GreaterThan(0).WithMessage("Order value cannot be less than 1!");
        }
    }
}
