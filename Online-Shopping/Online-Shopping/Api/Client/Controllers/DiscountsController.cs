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

namespace Online_Shopping.Api.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DiscountsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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


        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Discount> discounts = await _context.Discounts.ToListAsync();

            return Ok(_mapper.Map<List<DiscountItemDto>>(discounts));
        }
        #endregion
    }
}
