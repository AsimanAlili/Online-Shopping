using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName).MaximumLength(50).WithMessage("Lenght can not be 50!")
                .NotEmpty().NotNull().WithMessage("Can not be empty");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("Lenght can not be 100!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Password).MaximumLength(20).NotNull().NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Password and Confirm password are not same!");
        }
    }
}
