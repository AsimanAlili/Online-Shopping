using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class AdminEditDto
    {

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Photo { get; set; }
        public IFormFile File { get; set; }


    }
    public class AdminEditDtoValidator : AbstractValidator<AdminEditDto>
    {
        public AdminEditDtoValidator()
        {
            RuleFor(x => x.UserName).MaximumLength(100)
                .WithMessage("Lenght can not be 100!")
                 .NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.PhoneNumber).MaximumLength(50).WithMessage("Lenght can not be 50!");
            RuleFor(x => x.Photo).MaximumLength(100)
             .WithMessage("Lenght can not be 100!");
            RuleFor(x => x.CurrentPassword).MaximumLength(20).NotNull().NotEmpty();
            RuleFor(x => x.CurrentPassword).NotEqual(x => x.Password)
                .WithMessage("New Password and Current password can not be same!");
            RuleFor(x => x.Password).MaximumLength(20).NotNull().NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
                .WithMessage("Password and Confirm password are not same!");
        }
    }
}
