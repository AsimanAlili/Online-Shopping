using AutoMapper;
using Online_Shopping.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto createDto)
        {
            #region Checks
            //404
            #region CheckSubCategoryNotFound
            if (!await _context.SubCategories.AnyAsync(x => x.Id == createDto.SubCategoryId && !x.IsDeleted))
                return NotFound($"SubCategory not found by id: {createDto.SubCategoryId}");
            #endregion
            //404
            #region CheckBrandNotFound
            if (!await _context.Brands.AnyAsync(x => x.Id == createDto.BrandId))
                return NotFound($"Brand not found by id: {createDto.BrandId}");
            #endregion

            #region CheckProductExist
            if (await _context.Products.AnyAsync(x => x.Name.ToUpper() == createDto.Name.ToUpper().Trim()))
                return Conflict($"Product already exist by name {createDto.Name}");
            #endregion
            #endregion

            Product product = _mapper.Map<Product>(createDto);

            #region CheckFile
            int photoOrder = 1;
            foreach (var file in createDto.Files)
            {

                ProductPhoto productPhoto = new ProductPhoto();
                try
                {
                    productPhoto = _createProductPhoto(photoOrder, file);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Files", e.Message);
                }
                product.ProductPhotos.Add(productPhoto);
                photoOrder++;
            }
            #endregion

            product.DiscountedPrice = product.DiscountPercent <= 0 ? product.Price : (product.Price * (100 - product.DiscountPercent) / 100);
            if (product.Rate == 0)
                product.Rate = 5;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return StatusCode(201, product.Id);

        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _context.Products
               .Include(x=>x.Brand).Include(x=>x.SubCategory).Include(x=>x.ProductSizes).ThenInclude(x=>x.Size)
                .Include(x=>x.ProductColors).ThenInclude(x=>x.Color).Include(x=>x.ProductPhotos)
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
                .Skip((page - 1) * 10).Take(10).OrderByDescending(x=>x.CreatedAt).ToListAsync();

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

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductCreateDto editDto)
        {
            Product product = await _context.Products.
               Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
               .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
               .FirstOrDefaultAsync(x => x.Id == id);

            #region Ckecks
            //404
            #region CheckSubCategoryNotFound
            if (!await _context.SubCategories.AnyAsync(x => x.Id == editDto.SubCategoryId && !x.IsDeleted))
                return NotFound($"SubCategory not found by id: {editDto.SubCategoryId}");
            #endregion
            //404
            #region CheckBrandNotFound
            if (!await _context.Brands.AnyAsync(x => x.Id == editDto.BrandId))
                return NotFound($"Brand not found by id: {editDto.BrandId}");
            #endregion

            //NorFound
            #region CheckProductNotFound
            if (product == null)
                return NotFound();
            #endregion
            #endregion

            #region ProductSet

            product.ProductColors = await _getUpdatedProductColorsAsync(product.ProductColors, editDto.ProductColors, product.Id);
            product.ProductSizes = await _getUpdatedProductSizesAsync(product.ProductSizes, editDto.ProductSizes, product.Id);

            #region File
            List<ProductPhoto> removablePhotos = new List<ProductPhoto>();

            foreach (var item in product.ProductPhotos)
            {
                if (editDto.FileIds.Any(x => x == item.Id))
                    continue;

                FileManagerHelper.Delete(_env.WebRootPath, "uploads/products", item.Name);
                removablePhotos.Add(item);
            }
            product.ProductPhotos = product.ProductPhotos.Except(removablePhotos).ToList();

            var lastPhoto = product.ProductPhotos.OrderByDescending(x => x.Order).FirstOrDefault();
            int photoOrder = lastPhoto != null ? lastPhoto.Order + 1 : 1;


            foreach (var file in editDto.Files)
            {

                ProductPhoto productPhoto = new ProductPhoto();
                try
                {
                    productPhoto = _createProductPhoto(photoOrder, file);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Files", e.Message);
                }
                product.ProductPhotos.Add(productPhoto);
                photoOrder++;
            }
            #endregion

            if (product.Price != editDto.Price || product.DiscountPercent != editDto.DiscountPercent)
            {
                product.DiscountedPrice = editDto.DiscountPercent <= 0 ? editDto.Price : (editDto.Price * (100 - editDto.DiscountPercent) / 100);
            }

            product.Name = editDto.Name;
            product.Slug = editDto.Slug;
            product.Desc = editDto.Desc;
            product.Specification = editDto.Specification;
            product.Price = editDto.Price;
            product.ProducingPrice = editDto.ProducingPrice;
            product.DiscountPercent = editDto.DiscountPercent;
            product.IsAvailable = editDto.IsAvailable;
            product.IsBestSeller = editDto.IsBestSeller;
            product.IsFeature = editDto.IsFeature;
            product.IsHotTrend = editDto.IsHotTrend;
            product.IsNew = editDto.IsNew;
            product.Gender = editDto.Gender;
            product.BrandId = editDto.BrandId;
            product.SubCategoryId = editDto.SubCategoryId;
            #endregion

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.
               Include(x => x.Brand).Include(x => x.SubCategory).Include(x => x.ProductSizes).ThenInclude(x => x.Size)
               .Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.ProductPhotos)
               .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckProductNotFound
            if (product == null)
                return NotFound();
            #endregion

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();

        }
        #endregion

        #region GetUpdateProductColors
        private async Task<List<ProductColor>> _getUpdatedProductColorsAsync(List<ProductColor> productColors, List<ProductColorDto> colorDtos, int productId)
        {
            List<ProductColor> removableColors = new List<ProductColor>();
            removableColors.AddRange(productColors);
            #region CheckColors
            foreach (var colorDto in colorDtos)
            {
                ProductColor color = productColors.FirstOrDefault(x => x.ColorId == colorDto.ColorId);

                if (color != null)
                {
                    
                    removableColors.Remove(color);
                    color.IsAvailableColor = colorDto.IsAvailableColor;
                }
                else
                {
                    if (!await _context.Colors.AnyAsync(x => x.Id == colorDto.ColorId))
                    {
                        throw new Exception("Color not found!");
                    }

                    color = new ProductColor
                    {
                        ColorId = colorDto.ColorId,
                        ProductId = productId,
                        IsAvailableColor=colorDto.IsAvailableColor
                    };

                    productColors.Add(color);
                }
            }
            #endregion
            productColors = productColors.Except(removableColors).ToList();
            return productColors;
        }

        #endregion

        #region GetUpdateProductSizes
        private async Task<List<ProductSize>> _getUpdatedProductSizesAsync(List<ProductSize> productSizes, List<ProductSizeDto> sizeDtos, int productId)
        {
            List<ProductSize> removableSizes = new List<ProductSize>();
            removableSizes.AddRange(productSizes);
            #region CheckSizes
            foreach (var sizeDto in sizeDtos)
            {
                ProductSize size = productSizes.FirstOrDefault(x => x.SizeId == sizeDto.SizeId);

                if (size != null)
                {
                    removableSizes.Remove(size);
                    size.IsAvailableSize = sizeDto.IsAvailableSize;

                }
                else
                {
                    if (!await _context.Sizes.AnyAsync(x => x.Id == sizeDto.SizeId))
                    {
                        throw new Exception("Size not found!");
                    }

                    size = new ProductSize
                    {
                        SizeId = sizeDto.SizeId,
                        ProductId = productId,
                        IsAvailableSize = sizeDto.IsAvailableSize

                };

                    productSizes.Add(size);
                }
            }
            #endregion
            productSizes = productSizes.Except(removableSizes).ToList();
            return productSizes;
            
        }
        #endregion

        #region ProductPhotosCreate
        private ProductPhoto _createProductPhoto(int order, IFormFile file)
        {
            #region CheckFileLength
            if (file.Length > 2 * (1024 * 1024))
            {
                throw new Exception("File cannot be more than 4MB");
            }
            #endregion

            #region CheckFileContentType
            if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
            {
                throw new Exception("File only jpeg and png files accepted");
            }
            #endregion            

            string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/products", file);

            ProductPhoto productPhoto = new ProductPhoto
            {
                Name = filename,
                Order = order,
            };

            return productPhoto;
        }
        

        #endregion

        #region ProductReviewGetByProductId
        [HttpGet("review/{productId}")]

        public async Task<IActionResult> Review(int productId)
        {
            List<ProductReview> productReviews = await _context.ProductReviews.Where(x => x.ProductId == productId).ToListAsync();

            return Ok(productReviews);
        }

        #endregion

        #region ProductReviewDelete
        [HttpDelete("review/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            ProductReview review = await _context.ProductReviews.FirstOrDefaultAsync(x => x.Id == id);
            Product product = await _context.Products.Include(x => x.ProductReviews).FirstOrDefaultAsync(x => x.Id == review.ProductId);

            int reviewCount = (product.ProductReviews.Count() - 1);
            if (reviewCount == 0)
                reviewCount = 1;
            product.Rate = (product.ProductReviews.Sum(x => x.Rate) - review.Rate) / reviewCount;

            #region CheckReviewNotFound
            if (review == null)
                return NotFound();
            #endregion

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        #endregion
    }
}
