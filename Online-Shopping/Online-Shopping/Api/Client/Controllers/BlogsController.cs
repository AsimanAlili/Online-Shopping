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
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Blog blog = await _context.Blogs
               .Include(x => x.BlogCategory).Include(x => x.BlogTags).ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);

            #region CheckBlogNotFound
            if (blog == null)
                return NotFound();
            #endregion

            BlogGetDto blogGetDto = _mapper.Map<BlogGetDto>(blog);

            return Ok(blogGetDto);
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Blog> blogs = await _context.Blogs
               .Include(x => x.BlogCategory).Include(x => x.BlogTags).ThenInclude(x => x.Tag)
                .Skip((page - 1) * 10).Take(10).OrderByDescending(x => x.CreatedAt).ToListAsync();

            BlogClientListDto blogList = new BlogClientListDto
            {
                Blogs = _mapper.Map<List<BlogClientItemDto>>(blogs),
                TotalCount = await _context.Blogs.CountAsync()
            };

            return Ok(blogList);
        }
        #endregion
    }
}
