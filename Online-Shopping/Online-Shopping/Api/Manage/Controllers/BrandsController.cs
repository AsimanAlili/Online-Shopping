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
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandsController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(BrandCreateDto createDto)
        {
            #region CheckBrandExist
            if (await _context.Brands.AnyAsync(x => x.Name.ToUpper() == createDto.Name.ToUpper().Trim()))
                return Conflict($"Brand already exist by name {createDto.Name}");
            #endregion

            Brand brand = _mapper.Map<Brand>(createDto);
            brand.ModifiedAt = DateTime.UtcNow.AddHours(4);
            brand.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return StatusCode(201, brand.Id);
        }
        #endregion
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get (int id)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckBrandNotFound
            if (brand == null)
                return NotFound();
            #endregion
            BrandGetDto getDto = _mapper.Map<BrandGetDto>(brand);

            return Ok(getDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Brand> brands = await _context.Brands.Skip((page - 1) * 5).Take(5).ToListAsync();
            BrandListDto listDto = new BrandListDto
            {
                Brands = _mapper.Map<List<BrandItemDto>>(brands),
                TotalCount = await _context.Brands.CountAsync()
            };
            return Ok(listDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Brand> brands =await _context.Brands.ToListAsync();
            var map = _mapper.Map<List<BrandItemDto>>(brands);

            return Ok(map);
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit (int id,BrandCreateDto editDto)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubBrandExist
            if (await _context.Brands.AnyAsync(x => x.Name.ToUpper() == editDto.Name.ToUpper().Trim() && x.Id != id))
                return Conflict($"Brand already exist by name {editDto.Name}");
            #endregion
            #region CheckBrandNotFound
            if (brand == null)
                return NotFound();
            #endregion

            brand.Name = editDto.Name;
            brand.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(brand);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckBrandNotFound
            if (brand == null)
                return NotFound();
            #endregion

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
