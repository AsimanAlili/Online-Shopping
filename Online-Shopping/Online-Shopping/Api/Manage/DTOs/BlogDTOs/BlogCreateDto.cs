using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BlogCreateDto
    {
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Desc { get; set; }
        public List<BlogTagDto> BlogTags { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
    public class BlogTagDto
    {
        public int TagId { get; set; }
    }
    public class BlogCreateValidatorDto : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateValidatorDto()
        {
            RuleFor(x => x.Title).MaximumLength(250).WithMessage("Length cannot be greater than 250!")
               .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.FullName).MaximumLength(50).WithMessage("Length cannot be greater than 50!")
                .NotEmpty().NotNull().WithMessage("Cannot be empty!");
            RuleFor(x => x.Desc).MaximumLength(3000).WithMessage("Length cannot be greater than 3000!");
            RuleFor(x => x.BlogCategoryId).NotEmpty().NotNull().WithMessage("Cannot be empty!");
        }
    }
}
