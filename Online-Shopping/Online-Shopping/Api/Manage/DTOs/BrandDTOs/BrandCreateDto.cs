using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BrandCreateDto
    {
        public string Name { get; set; }
    }
    public class BrandCreateValidatorDto : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateValidatorDto()
        {
            RuleFor(x=>x.Name).MaximumLength(50).WithMessage("Length cannot be greater than 50!")
               .NotEmpty().NotNull().WithMessage("Cannot be empty!");
        }
    }
}
