using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Client.DTOs;
using Online_Shopping.Api.Manage.DTOs;
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
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _context.Products
               .Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);

            #region CheckProductNotFound
            if (product == null)
                return NotFound();
            #endregion

            List<ProductColor> pc = product.ProductColors.Where(x => !x.IsAvailableColor).ToList();
            product.ProductColors = pc;

            List<ProductSize> ps = product.ProductSizes.Where(x => !x.IsAvailableSize).ToList();
            product.ProductSizes = ps;



            ProductGetDto productGetDto = _mapper.Map<ProductGetDto>(product);

            return Ok(productGetDto);


        }
        #endregion
        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Product> products = await _context.Products.
                Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
                .Skip((page - 1) * 10).Take(10).OrderByDescending(x => x.CreatedAt).ToListAsync();

            foreach (var product in products)
            {
                List<ProductColor> pc = product.ProductColors.Where(x => !x.IsAvailableColor).ToList();
                product.ProductColors = pc;
                List<ProductSize> ps = product.ProductSizes.Where(x => !x.IsAvailableSize).ToList();
                product.ProductSizes = ps;
            }


            ProductListDto productList = new ProductListDto
            {
                Products = _mapper.Map<List<ProductItemDto>>(products),
                TotalCount = await _context.Products.CountAsync()
            };

            return Ok(productList);
        }
        #endregion
        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Product> products = await _context.Products.
                Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

            foreach (var product in products)
            {
                List<ProductColor> pc = product.ProductColors.Where(x => !x.IsAvailableColor).ToList();
                product.ProductColors = pc;
                List<ProductSize> ps = product.ProductSizes.Where(x => !x.IsAvailableSize).ToList();
                product.ProductSizes = ps;
            }
            
            return Ok(_mapper.Map<List<ProductItemDto>>(products));
        }
        #endregion
        #region GetNew
        [HttpGet("all")]
        public async Task<IActionResult> GetNew()
        {
            List<Product> products = await _context.Products.
                Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
                .Where(x=>x.IsNew).Take(8).OrderByDescending(x => x.CreatedAt).ToListAsync();

            foreach (var product in products)
            {
                List<ProductColor> pc = product.ProductColors.Where(x => !x.IsAvailableColor).ToList();
                product.ProductColors = pc;
                List<ProductSize> ps = product.ProductSizes.Where(x => !x.IsAvailableSize).ToList();
                product.ProductSizes = ps;
            }

            return Ok(_mapper.Map<List<ProductItemDto>>(products));
        }
        #endregion
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

        #region Filter
        [HttpPost("filter")]
        public async Task<IActionResult> GetFilter(ProductFilterItemDto productFilter, int page = 1)
        {
            List<Product> products = await _context.Products
                .Include(x => x.SubCategory).Where(x =>(x.Price > productFilter.MinPrice && x.Price < productFilter.MaxPrice)
                || x.SubCategoryId == productFilter.SubCategoryId).Skip((page - 1) * 8).Take(8).ToListAsync();

            ProductListFilterDto productDtos = new ProductListFilterDto
            {
                Products = _mapper.Map<List<ProductFilterItemDto>>(products),
                TotalCount = products.Count()
            };

            return Ok(productDtos);
        }
        #endregion

        #region Search
        [HttpPost("search")]
        public async Task<IActionResult> GetSearch(ProductSearchItemDto productSearch, int page = 1)
        {
            List<Product> products = await _context.Products
                .Include(x => x.SubCategory).Include(x => x.Brand)
                .Where(x => x.Name.Contains(productSearch.Name)
                || x.SubCategory.Name.Contains(productSearch.SubCategoryName) || x.Brand.Name.Contains(productSearch.BrandName))
                .Skip((page - 1) * 8).Take(8).ToListAsync();

            ProductSearchListDto productDtos = new ProductSearchListDto
            {
                Products = _mapper.Map<List<ProductSearchItemDto>>(products),
                TotalCount = products.Count()

            };

            return Ok(productDtos);
        }
        #endregion

        #region Bookmark
        [HttpPut("bookmark/{id}")]
        public async Task<IActionResult> Bookmark(int id, ProductBookmarkDto productBookmarkDto)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.IsBookmarked != productBookmarkDto.IsBookmarked)
            {
                product.IsBookmarked = productBookmarkDto.IsBookmarked;
            }
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }
        #endregion
    }
}
