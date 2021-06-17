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
    public class DiscountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public DiscountsController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] DiscountCreateDto createDto)
        {
            Discount discount = _mapper.Map<Discount>(createDto);

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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/discounts", createDto.File);

                discount.Photo = filename;
            }
            #endregion

            discount.CreatedAt = DateTime.UtcNow.AddHours(4);
            discount.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return StatusCode(201, discount.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Discount discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckDiscountNotFound
            if (discount == null)
                return NotFound();
            #endregion
            DiscountGetDto discountGetDto = _mapper.Map<DiscountGetDto>(discount);

            return Ok(discountGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Discount> discounts = await _context.Discounts.Skip((page - 1) * 8).Take(8).ToListAsync();

            DiscountListDto discountsDto = new DiscountListDto
            {
                Discounts = _mapper.Map<List<DiscountItemDto>>(discounts),
                TotalCount = await _context.Discounts.CountAsync()
            };

            return Ok(discountsDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Discount> discounts = await _context.Discounts.ToListAsync();

            return Ok(_mapper.Map<List<DiscountItemDto>>(discounts));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] DiscountCreateDto editDto)
        {
            Discount discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);

            
            #region CheckDiscountNotFound
            if (discount == null)
                return NotFound();
            #endregion
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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/discounts", editDto.File);
                if (!string.IsNullOrWhiteSpace(discount.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/discounts", discount.Photo);
                }
                discount.Photo = filename;
            }

            #endregion

            discount.Title = editDto.Title;
            discount.SubTitle = editDto.SubTitle;
            discount.SaleTitle = editDto.SaleTitle;
            discount.RedirectUrl = editDto.RedirectUrl;
            discount.DiscountTime = editDto.DiscountTime;
            discount.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(discount);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Discount discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckDiscountNotFound
            if (discount == null)
                return NotFound();
            #endregion
            _context.Discounts.Remove(discount);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
