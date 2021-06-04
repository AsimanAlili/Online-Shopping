using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ColorCreateDto
    {
        public string Name { get; set; }

    }
    public class ColorCreateValidatorDto : AbstractValidator<ColorCreateDto>
    {
        public ColorCreateValidatorDto()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Length cannot be greater than 50!")
               .NotEmpty().NotNull().WithMessage("Cannot be empty!");
        }
    }
}
