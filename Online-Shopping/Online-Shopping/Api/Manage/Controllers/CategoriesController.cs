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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]  CategoryCreateDto createDto)
        {
            #region CheckCategoryExist
            if (await _context.Categories.AnyAsync(x => x.Name.ToUpper() == createDto.Name.ToUpper().Trim()))
                return Conflict($"Category already exist by name{createDto.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.Categories.AnyAsync(x => x.Order == createDto.Order))
                return Conflict($"Category Already exist by order {createDto.Order}");
            #endregion

            Category category = _mapper.Map<Category>(createDto);

            #region CheckFile
            if (createDto.File != null)
            {
                #region CheckPhotoLength
                if (createDto.File.Length > 4 * (1024 * 1024))
                {
                    return StatusCode(409, "File cannot be more than 4MB");
                }
                #endregion
                #region CheckPhotoContentType
                if (createDto.File.ContentType != "image/png" && createDto.File.ContentType != "image/jpeg")
                {
                    return StatusCode(409, "File only jpeg and png files accepted");
                }
                #endregion

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/categories", createDto.File);

                category.Photo = filename;
            }
            #endregion

            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            category.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return StatusCode(201, category.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion
            CategoryGetDto categoryGetDto = _mapper.Map<CategoryGetDto>(category);

            return Ok(categoryGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Category> categories = await _context.Categories.Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Order).Skip((page - 1) * 8).Take(8).ToListAsync();

            CategoryListDto categoriesDto = new CategoryListDto
            {
                Categories = _mapper.Map<List<CategoryItemDto>>(categories),
                TotalCount = await _context.Categories.Where(x => !x.IsDeleted).CountAsync()
            };

            return Ok(categoriesDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _context.Categories.Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Order).ToListAsync();

            return Ok(_mapper.Map<List<CategoryItemDto>>(categories));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] CategoryCreateDto editDto)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckCategoryExist
            if (await _context.Categories.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"Category already exist by name {editDto.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.Categories.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"Category Already exist by order {editDto.Order}");
            #endregion
            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion
            #region CheckFile
            if (editDto.File != null)
            {
                #region CheckPhotoLength
                if (editDto.File.Length > 4 * (1024 * 1024))
                {
                    return StatusCode(409, "File cannot be more than 4MB");
                }
                #endregion
                #region CheckPhotoContentType
                if (editDto.File.ContentType != "image/png" && editDto.File.ContentType != "image/jpeg")
                {
                    return StatusCode(409, "File only jpeg and png files accepted");
                }
                #endregion

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/categories", editDto.File);
                if (!string.IsNullOrWhiteSpace(category.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/categories", category.Photo);
                }
                category.Photo = filename;
            }

            #endregion

            category.Name = editDto.Name;
            category.Order = editDto.Order;
            category.Desc = editDto.Desc;
            category.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(category);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion
            category.IsDeleted = true;

            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
