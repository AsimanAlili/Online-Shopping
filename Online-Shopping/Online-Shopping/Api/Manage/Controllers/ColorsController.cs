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
    public class ColorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ColorsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(ColorCreateDto createDto)
        {
            #region CheckColorExist
            if (await _context.Colors.AnyAsync(x => x.Name.ToUpper() == createDto.Name.ToUpper().Trim()))
                return Conflict($"Color already exist by name {createDto.Name}");
            #endregion

            Color color = _mapper.Map<Color>(createDto);
            color.ModifiedAt = DateTime.UtcNow.AddHours(4);
            color.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return StatusCode(201, color.Id);
        }
        #endregion
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckColorNotFound
            if (color == null)
                return NotFound();
            #endregion
            ColorGetDto getDto = _mapper.Map<ColorGetDto>(color);

            return Ok(getDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Color> colors = await _context.Colors.Skip((page - 1) * 5).Take(5).ToListAsync();
            ColorListDto listDto = new ColorListDto
            {
                Colors = _mapper.Map<List<ColorItemDto>>(colors),
                TotalCount = await _context.Colors.CountAsync()
            };
            return Ok(listDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Color> colors = await _context.Colors.ToListAsync();
            var map = _mapper.Map<List<ColorItemDto>>(colors);

            return Ok(map);
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, ColorCreateDto editDto)
        {
            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubColorExist
            if (await _context.Colors.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"Color already exist by name {editDto.Name}");
            #endregion
            #region CheckColorNotFound
            if (color == null)
                return NotFound();
            #endregion

            color.Name = editDto.Name;
            color.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(color);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckColorNotFound
            if (color == null)
                return NotFound();
            #endregion

            _context.Colors.Remove(color);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
