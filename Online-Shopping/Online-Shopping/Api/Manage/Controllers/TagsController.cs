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
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagsController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(TagCreateDto tagCreate)
        {
            #region CheckTagExist
            if (await _context.Tags.AnyAsync(x => x.Name.ToUpper() == tagCreate.Name.ToUpper().Trim()))
                return Conflict($"Tag already exist by name{tagCreate.Name}");
            #endregion

            Tag tag = _mapper.Map<Tag>(tagCreate);
            tag.CreatedAt = DateTime.UtcNow.AddHours(4);
            tag.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return StatusCode(201, tag.Id);
        }
        #endregion
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckTagNotFound
            if (tag == null)
                return NotFound();
            #endregion
            TagGetDto tagGetDto = _mapper.Map<TagGetDto>(tag);

            return Ok(tagGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Tag> tags = await _context.Tags.Skip((page - 1) * 8).Take(8).ToListAsync();

            TagListDto tagsDto = new TagListDto
            {
                Tags = _mapper.Map<List<TagItemDto>>(tags),
                TotalCount = await _context.Tags.CountAsync()
            };

            return Ok(tagsDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();

            return Ok(_mapper.Map<List<TagItemDto>>(tags));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, TagCreateDto editDto)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubTagExist
            if (await _context.Tags.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"Tag already exist by name {editDto.Name}");
            #endregion
           
            #region CheckTagNotFound
            if (tag == null)
                return NotFound();
            #endregion 

            tag.Name = editDto.Name;
            tag.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(tag);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckTagNotFound
            if (tag == null)
                return NotFound();
            #endregion

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return NoContent();

        }
        #endregion
    }
}
