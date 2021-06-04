using AutoMapper;
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

        public CategoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreate)
        {
            #region CheckCategoryExist
            if (await _context.Categories.AnyAsync(x => x.Name.ToUpper() == categoryCreate.Name.ToUpper().Trim()))
                return Conflict($"Category already exist by name{categoryCreate.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.Categories.AnyAsync(x => x.Order == categoryCreate.Order))
                return Conflict($"Category Already exist by order {categoryCreate.Order}");
            #endregion

            Category category = _mapper.Map<Category>(categoryCreate);
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
        public async Task<IActionResult> Edit(int id, CategoryCreateDto editDto)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckSubCategoryExist
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

            category.Name = editDto.Name;
            category.Order = editDto.Order;
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
