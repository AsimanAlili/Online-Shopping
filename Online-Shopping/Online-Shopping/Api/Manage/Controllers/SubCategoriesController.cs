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
    public class SubCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SubCategoriesController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(SubCategoryCreateDto createDto)
        {
            #region CheckSubCategoryExist
            if (await _context.SubCategories.AnyAsync(x => x.Name.ToUpper() == createDto.Name.ToUpper().Trim()))
                return Conflict($"SubCategory already exist by name {createDto.Name}");
            #endregion

            #region CheckCategoryNotFound
            if (!await _context.Categories.AnyAsync(x => x.Id == createDto.CategoryId))
                return NotFound($"Category not found by id: {createDto.CategoryId}");
            #endregion
            #region CheckOrderExist
            if (await _context.SubCategories.AnyAsync(x => x.Order == createDto.Order))
                return Conflict($"Subcategory Already exist by order {createDto.Order}");
            #endregion


            SubCategory subCategory = _mapper.Map<SubCategory>(createDto);
            subCategory.CreatedAt = DateTime.UtcNow.AddHours(4);
            subCategory.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();

            return StatusCode(201, subCategory.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SubCategory subCategory = await _context.SubCategories.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckSubCategoryNotFound
            if (subCategory == null)
                return NotFound();
            #endregion
            SubCategoryGetDto getDto = _mapper.Map<SubCategoryGetDto>(subCategory);

            return Ok(getDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<SubCategory> subCategories = await _context.SubCategories
                 .Include(x => x.Category).Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Order).Skip((page - 1) * 10).Take(10).ToListAsync();


            SubCategoryListDto subCategoryDto = new SubCategoryListDto
            {
                SubCategories = _mapper.Map<List<SubCategoryItemDto>>(subCategories),
                TotalCount = await _context.SubCategories.CountAsync()
            };

            return Ok(subCategoryDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<SubCategory> subCategories = await _context.SubCategories.Include(x=>x.Category).Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Order).ToListAsync();

            return Ok(_mapper.Map<List<SubCategoryItemDto>>(subCategories));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,SubCategoryCreateDto editDto)
        {
            SubCategory subCategory = await _context.SubCategories.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckSubCategoryExist
            if (await _context.SubCategories.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id!=id))
                return Conflict($"SubCategory already exist by name {editDto.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.SubCategories.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"Subcategory Already exist by order {editDto.Order}");
            #endregion
            #region CheckCategoryNotFound
            if (!await _context.Categories.AnyAsync(x => x.Id == editDto.CategoryId))
                return NotFound($"Category not found by id: {editDto.CategoryId}");
            #endregion
            #region CheckSubCategoryNotFound
            if (subCategory == null)
                return NotFound();
            #endregion

            subCategory.Name = editDto.Name;
            subCategory.Order = editDto.Order;
            subCategory.CategoryId = editDto.CategoryId;
            subCategory.CreatedAt = DateTime.UtcNow.AddHours(4);
            subCategory.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            SubCategory subCategory = await _context.SubCategories.Include(x=>x.Category).FirstOrDefaultAsync(x => x.Id == id);

            #region CheckCategoryNotFound
            if (subCategory == null)
                return NotFound();
            #endregion
            subCategory.IsDeleted = true;

            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
