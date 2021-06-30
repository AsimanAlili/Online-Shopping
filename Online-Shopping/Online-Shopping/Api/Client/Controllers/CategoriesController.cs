using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Client.DTOs;
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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public CategoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;       
        }
        #region Get
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion
            CategoryClientGetDto categoryGetDto = _mapper.Map<CategoryClientGetDto>(category);

            return Ok(categoryGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Category> categories = await _context.Categories.Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Order).Skip((page - 1) * 8).Take(8).ToListAsync();

            CategoryClientListDto categoriesDto = new CategoryClientListDto
            {
                Categories = _mapper.Map<List<CategoryClientItemDto>>(categories),
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

            return Ok(_mapper.Map<List<CategoryClientItemDto>>(categories));
        }
        #endregion
    }
}
