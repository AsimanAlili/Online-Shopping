using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using Online_Shopping.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public BlogsController(AppDbContext context,IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto createDto)
        {
            #region Checks
            //404
            #region CheckBlogCategoryNotFound
            if (!await _context.BlogCategories.AnyAsync(x => x.Id == createDto.BlogCategoryId))
                return NotFound($"BlogCategory not found by id: {createDto.BlogCategoryId}");
            #endregion

            #region CheckBlogExist
            if (await _context.Blogs.AnyAsync(x => x.Title.ToUpper() == createDto.Title.ToUpper().Trim()))
                return Conflict($"Blog already exist by name {createDto.Title}");
            #endregion
            #endregion

            Blog blog = _mapper.Map<Blog>(createDto);

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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/blogs", createDto.File);

                blog.Photo = filename;
            }
            #endregion
           
            blog.CreatedAt = DateTime.UtcNow.AddHours(4);
            blog.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return StatusCode(201, blog.Id);
        }
        #endregion

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
                .Skip((page - 1) * 10).Take(10).OrderByDescending(x=>x.CreatedAt).ToListAsync();

            BlogListDto blogList = new BlogListDto
            {
                Blogs = _mapper.Map<List<BlogItemDto>>(blogs),
                TotalCount = await _context.Blogs.CountAsync()
            };

            return Ok(blogList);
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] BlogCreateDto editDto)
        {
            Blog blog = await _context.Blogs.
               Include(x => x.BlogCategory).Include(x => x.BlogTags).ThenInclude(x => x.Tag)
               .FirstOrDefaultAsync(x => x.Id == id);

            #region Ckecks
            //404
            #region CheckBlogCategoryNotFound
            if (!await _context.BlogCategories.AnyAsync(x => x.Id == editDto.BlogCategoryId))
                return NotFound($"BlogCategory not found by id: {editDto.BlogCategoryId}");
            #endregion
            #region CheckBlogExist
            if (await _context.Blogs.AnyAsync(x => x.Title.ToUpper() == editDto.Title.ToUpper().Trim() && x.Id != id))
                return Conflict($"Blog already exist by name {editDto.Title}");
            #endregion
            //NorFound
            #region CheckBlogNotFound
            if (blog == null)
                return NotFound();
            #endregion

            #endregion

            #region BlogSet

            blog.BlogTags = await _getUpdatedBlogTagsAsync(blog.BlogTags, editDto.BlogTags, blog.Id);

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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/blogs", editDto.File);
                if (!string.IsNullOrWhiteSpace(blog.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/blogs", blog.Photo);
                }
                blog.Photo = filename;
            }
            #endregion

            blog.Title = editDto.Title;
            blog.FullName = editDto.FullName;
            blog.Desc = editDto.Desc;
            blog.BlogCategoryId = editDto.BlogCategoryId;
            #endregion

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _context.Blogs.
               Include(x => x.BlogCategory).Include(x => x.BlogTags).ThenInclude(x => x.Tag)
               .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckBlogNotFound
            if (blog == null)
                return NotFound();
            #endregion

            _context.Blogs.Remove(blog);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion

        #region GetUpdateBlogTags
        private async Task<List<BlogTag>> _getUpdatedBlogTagsAsync(List<BlogTag> blogTags, List<BlogTagDto> tagDtos, int blogId)
        {
            List<BlogTag> removableTags = new List<BlogTag>();
            removableTags.AddRange(blogTags);
            #region CheckTags
            foreach (var tagDto in tagDtos)
            {
                BlogTag tag = blogTags.FirstOrDefault(x => x.TagId == tagDto.TagId);

                if (tag != null)
                {

                    removableTags.Remove(tag);
                }
                else
                {
                    if (!await _context.Tags.AnyAsync(x => x.Id == tagDto.TagId))
                    {
                        throw new Exception("Tag not found!");
                    }

                    tag = new BlogTag
                    {
                        TagId = tagDto.TagId,
                        BlogId = blogId,
                    };

                    blogTags.Add(tag);
                }
            }
            #endregion
            blogTags = blogTags.Except(removableTags).ToList();
            return blogTags;
        }

        #endregion
    }
}
