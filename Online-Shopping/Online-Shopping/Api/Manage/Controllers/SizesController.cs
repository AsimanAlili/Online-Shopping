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
    public class SizesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SizesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(SizeCreateDto sizeCreate)
        {
            #region CheckSizeExist
            if (await _context.Sizes.AnyAsync(x => x.Name.ToUpper() == sizeCreate.Name.ToUpper().Trim()))
                return Conflict($"Size already exist by name{sizeCreate.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.Sizes.AnyAsync(x => x.Order == sizeCreate.Order))
                return Conflict($"Size Already exist by order {sizeCreate.Order}");
            #endregion

            Size size = _mapper.Map<Size>(sizeCreate);
            size.CreatedAt = DateTime.UtcNow.AddHours(4);
            size.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return StatusCode(201, size.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSizeNotFound
            if (size == null)
                return NotFound();
            #endregion
            SizeGetDto sizeGetDto = _mapper.Map<SizeGetDto>(size);

            return Ok(sizeGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Size> sizes = await _context.Sizes
                .OrderByDescending(x => x.Order).Skip((page - 1) * 8).Take(8).ToListAsync();

            SizeListDto sizesDto = new SizeListDto
            {
                Sizes = _mapper.Map<List<SizeItemDto>>(sizes),
                TotalCount = await _context.Sizes.CountAsync()
            };

            return Ok(sizesDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Size> sizes = await _context.Sizes
                .OrderByDescending(x => x.Order).ToListAsync();

            return Ok(_mapper.Map<List<SizeItemDto>>(sizes));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, SizeCreateDto editDto)
        {
            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubSizeExist
            if (await _context.Sizes.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"Size already exist by name {editDto.Name}");
            #endregion
            #region CheckOrderExist
            if (await _context.Sizes.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"Size Already exist by order {editDto.Order}");
            #endregion
            #region CheckSizeNotFound
            if (size == null)
                return NotFound();
            #endregion 

            size.Name = editDto.Name;
            size.Order = editDto.Order;
            size.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(size);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSizeNotFound
            if (size == null)
                return NotFound();
            #endregion

            _context.Sizes.Remove(size);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
