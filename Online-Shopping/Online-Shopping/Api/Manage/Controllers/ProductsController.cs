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
          

            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return StatusCode(201, product.Id);

        }
        #endregion

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
    }
}
