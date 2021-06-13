using FluentValidation;
using Online_Shopping.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class OrderCreateDto
    {
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public OrderStatus Status { get; set; }
    }
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.AppUserId).NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.ProductId).NotEmpty().NotNull().WithMessage("Cannot be empty!");
        }
    }
}
