using AutoMapper;
using JobbApi.Helpers;
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
            

            #region CheckFile
            createDto.ProductPhotos = new List<ProductPhotoDto>();
            int photoOrder = 1;
            foreach (var file in createDto.Files)
            {

                ProductPhotoDto productPhoto = new ProductPhotoDto();
                try
                {
                    productPhoto = _createProductPhoto(photoOrder, file);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Files", e.Message);
                }
                createDto.ProductPhotos.Add(productPhoto);
                photoOrder++;
            }
            #endregion

            Product product = _mapper.Map<Product>(createDto);
            product.DiscountedPrice = product.DiscountPercent <= 0 ? product.Price : (product.Price * (100 - product.DiscountPercent) / 100);
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.ModifiedAt = DateTime.UtcNow.AddHours(4);
            //product.ProductColors.FirstOrDefault(pc => pc.ColorId == 2).ColorCount = createDto.ProductColors.FirstOrDefault().Count;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return StatusCode(201, product.Id);

        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //Product pr = _context.Products.FirstOrDefault(p => p.ProductColors.Where(pc => !pc.IsAvailableColor).Any());
            Product product = await _context.Products
               .Include(x=>x.Brand).Include(x=>x.SubCategory).Include(x=>x.ProductSizes).ThenInclude(x=>x.Size)
                .Include(x=>x.ProductColors).ThenInclude(x=>x.Color).Include(x=>x.ProductPhotos)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsAvailable);

            List<ProductColor> pc = product.ProductColors.Where(x => !x.IsAvailableColor).ToList();
            product.ProductColors = pc;

            List<ProductSize> ps = product.ProductSizes.Where(x => !x.IsAvailableSize).ToList();
            product.ProductSizes = ps;

            #region CheckCategoryNotFound
            if (product == null)
                return NotFound();
            #endregion


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
                .Skip((page - 1) * 10).Take(10).ToListAsync();

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
            //Conflict
            //#region CheckProductExist
            //if (await _context.Products.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
            //    return Conflict($"Product already exist by name {editDto.Name}");
            //#endregion

            //NorFound
            #region CheckProductNotFound
            if (product == null)
                return NotFound();
            #endregion
            #endregion

            product.ProductColors = await _getUpdatedProductColorsAsync(product.ProductColors, editDto.ProductColors.Select(x=>x.ColorId).ToList(), product.Id );
            product.ProductSizes = await _getUpdatedProductSizesAsync(product.ProductSizes, editDto.ProductSizes.Select(x => x.SizeId).ToList(), product.Id);


            //editDto.ProductPhotos = new List<ProductPhotoDto>();
            //int photoOrder = 1;
            //foreach (var file in editDto.Files)
            //{

            //    ProductPhotoDto productPhoto = new ProductPhotoDto();
            //    try
            //    {
            //        productPhoto = _createProductPhoto(photoOrder, file);
            //    }
            //    catch (Exception e)
            //    {
            //        ModelState.AddModelError("Files", e.Message);
            //    }
            //    editDto.ProductPhotos.Add(productPhoto);
            //    photoOrder++;
            //}
            #region File
            List<ProductPhoto> removablePhotos = new List<ProductPhoto>();
            var productP = product.ProductPhotos.Select(x => x.Id).ToList();
            editDto.FileIds = productP;

            foreach (var item in product.ProductPhotos)
            {
                if (editDto.FileIds.Any(x => x == item.Id))
                    continue;

                FileManagerHelper.Delete(_env.WebRootPath, "uploads/products", item.Name);
                removablePhotos.Add(item);
            }
            product.ProductPhotos = product.ProductPhotos.Except(removablePhotos).ToList();

            var lastPhoto = editDto.ProductPhotos.OrderByDescending(x => x.Order).FirstOrDefault();
            int photoOrder = lastPhoto != null ? lastPhoto.Order + 1 : 1;


            foreach (var file in editDto.Files)
            {

                ProductPhotoDto productPhoto = new ProductPhotoDto();
                try
                {
                    productPhoto = _createProductPhoto(photoOrder, file);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Files", e.Message);
                }
                editDto.ProductPhotos.Add(productPhoto);
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
            #region CheckJobNotFound
            if (product == null)
                return NotFound();
            #endregion

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();

        }
        #endregion

        #region GetUpdateProductColors
        private async Task<List<ProductColor>> _getUpdatedProductColorsAsync(List<ProductColor> productColors, List<int> colorIds, int productId)
        {
            List<ProductColor> removableColors = new List<ProductColor>();
            removableColors.AddRange(productColors);
            #region CheckColors
            foreach (var colorId in colorIds)
            {
                ProductColor color = productColors.FirstOrDefault(x => x.ColorId == colorId);

                if (color != null)
                {
                    removableColors.Remove(color);
                }
                else
                {
                    if (!await _context.Colors.AnyAsync(x => x.Id == colorId))
                    {
                        throw new Exception("Color not found!");
                    }

                    color = new ProductColor
                    {
                        ColorId = colorId,
                        ProductId = productId,
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
        private async Task<List<ProductSize>> _getUpdatedProductSizesAsync(List<ProductSize> productSizes, List<int> sizeIds, int productId)
        {
            List<ProductSize> removableSizes = new List<ProductSize>();
            removableSizes.AddRange(productSizes);
            #region CheckSizes
            foreach (var sizeId in sizeIds)
            {
                ProductSize size = productSizes.FirstOrDefault(x => x.SizeId == sizeId);

                if (size != null)
                {
                    removableSizes.Remove(size);
                }
                else
                {
                    if (!await _context.Sizes.AnyAsync(x => x.Id == sizeId))
                    {
                        throw new Exception("Size not found!");
                    }

                    size = new ProductSize
                    {
                        SizeId = sizeId,
                        ProductId = productId,
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
        private ProductPhotoDto _createProductPhoto(int order, IFormFile file)
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

            ProductPhotoDto productPhoto = new ProductPhotoDto
            {
                Name = filename,
                Order = order,
            };

            return productPhoto;
        }
        private ProductPhoto _createProductPhotoEdit(int order, IFormFile file)
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
    }
}
