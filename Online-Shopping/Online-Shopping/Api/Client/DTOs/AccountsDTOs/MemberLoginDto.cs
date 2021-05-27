using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class MemberLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class MemberLoginDtoValidator : AbstractValidator<MemberLoginDto>
    {
        public MemberLoginDtoValidator()
        {
            RuleFor(x => x.Email).MaximumLength(100)
                .WithMessage("Lenght cannot be greater than 100!")
                 .NotNull().NotEmpty().WithMessage("Cannot be empty");
            RuleFor(x => x.Password).MaximumLength(20).NotNull().NotEmpty().WithMessage("Cannot be empty");
        }
    }
}
