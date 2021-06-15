using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region ProductReview
        [HttpPost("review")]
        public async Task<IActionResult> Review(ProductReview review)
        {
            Product product = await _context.Products.Include(x => x.ProductReviews).FirstOrDefaultAsync(x => x.Id == review.ProductId);

            #region CheckProductNotFound
            if (product == null)
                return NotFound($"Product not found by id: {review.ProductId}");
            #endregion

            ProductReview productReview = new ProductReview
            {
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Email = review.Email,
                FullName = review.FullName,
                Rate = review.Rate,
                ProductId = review.ProductId,
                Message = review.Message
            };

            product.ProductReviews.Add(productReview);
            product.Rate = product.ProductReviews.Sum(x => x.Rate) / product.ProductReviews.Count();

            await _context.SaveChangesAsync();

            return StatusCode(201,review);
        }
        #endregion
    }
}
