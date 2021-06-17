using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ContactCreateDto
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Support { get; set; }
    }
    public class ContactCreateDtoValidator : AbstractValidator<ContactCreateDto>
    {
        public ContactCreateDtoValidator()
        {
            RuleFor(x => x.Phone).MaximumLength(150).WithMessage("Length cannot be greater than 150!")
               .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Address).MaximumLength(150).WithMessage("Length cannot be greater than 150!")
              .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Support).MaximumLength(150).WithMessage("Length cannot be greater than 150!")
              .NotEmpty().NotNull().WithMessage("Cannot be empty!");
        }
    }
}
