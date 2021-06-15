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
    public class BlogCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogCategoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(BlogCategoryCreateDto blogCategoryCreate)
        {
            #region CheckBlogCategoryExist
            if (await _context.BlogCategories.AnyAsync(x => x.Name.ToUpper() == blogCategoryCreate.Name.ToUpper().Trim()))
                return Conflict($"BlogCategory already exist by name{blogCategoryCreate.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.BlogCategories.AnyAsync(x => x.Order == blogCategoryCreate.Order))
                return Conflict($"BlogCategory Already exist by order {blogCategoryCreate.Order}");
            #endregion

            BlogCategory blogCategory = _mapper.Map<BlogCategory>(blogCategoryCreate);
            blogCategory.CreatedAt = DateTime.UtcNow.AddHours(4);
            blogCategory.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.BlogCategories.AddAsync(blogCategory);
            await _context.SaveChangesAsync();

            return StatusCode(201, blogCategory.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckBlogCategoryNotFound
            if (blogCategory == null)
                return NotFound();
            #endregion
            BlogCategoryGetDto blogCategoryGetDto = _mapper.Map<BlogCategoryGetDto>(blogCategory);

            return Ok(blogCategoryGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<BlogCategory> blogCategories = await _context.BlogCategories
                .OrderByDescending(x => x.Order).Skip((page - 1) * 8).Take(8).ToListAsync();

            BlogCategoryListDto blogCategoriesDto = new BlogCategoryListDto
            {
                BlogCategories = _mapper.Map<List<BlogCategoryItemDto>>(blogCategories),
                TotalCount = await _context.BlogCategories.CountAsync()
            };

            return Ok(blogCategoriesDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<BlogCategory> blogCategories = await _context.BlogCategories
                .OrderByDescending(x => x.Order).ToListAsync();

            return Ok(_mapper.Map<List<BlogCategoryItemDto>>(blogCategories));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, BlogCategoryCreateDto editDto)
        {
            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckBlogCategoryExist
            if (await _context.BlogCategories.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"BlogCategory already exist by name {editDto.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.BlogCategories.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"BlogCategory Already exist by order {editDto.Order}");
            #endregion
            #region CheckBlogCategoryNotFound
            if (blogCategory == null)
                return NotFound();
            #endregion 

            blogCategory.Name = editDto.Name;
            blogCategory.Order = editDto.Order;
            blogCategory.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(blogCategory);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckBlogCategoryNotFound
            if (blogCategory == null)
                return NotFound();
            #endregion

            _context.BlogCategories.Remove(blogCategory);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
