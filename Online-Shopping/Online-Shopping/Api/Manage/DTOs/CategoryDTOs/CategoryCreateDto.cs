using FluentValidation;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Length cannot be greater than 50!")
                .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Order).GreaterThan(0).WithMessage("Order value cannot be less than 1!");
        }
    }
}
