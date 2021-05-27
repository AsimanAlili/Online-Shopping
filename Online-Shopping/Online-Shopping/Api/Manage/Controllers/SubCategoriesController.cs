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
                return Conflict($"Category already exist by name{createDto.Name}");
            #endregion

            #region CheckCategoryNotFound
            if (!await _context.Categories.AnyAsync(x => x.Id == createDto.CategoryId))
                return NotFound($"Category not found by id: {createDto.CategoryId}");
            #endregion

            SubCategory subCategory = _mapper.Map<SubCategory>(createDto);
            subCategory.CreatedAt = DateTime.UtcNow.AddHours(4);
            subCategory.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();

            return StatusCode(201, subCategory.Id);
        }
        #endregion
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SubCategory subCategory = await _context.SubCategories.Include(x => x.Category ).FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubCategoryNotFound
            if (subCategory == null)
                return NotFound();
            #endregion
            SubCategoryGetDto getDto = _mapper.Map<SubCategoryGetDto>(subCategory);

            return Ok(getDto);
        }

    }
}
