using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public OrdersController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(OrderCreateDto createDto)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == createDto.ProductId);
            #region CheckProductNotFound
            if (product == null)
                return NotFound($"Product not found by id: {createDto.ProductId}");
            #endregion


            Order order = _mapper.Map<Order>(createDto);
            order.Price = product.Price;
            order.DiscountedPrice = product.DiscountedPrice;
            order.ModifiedAt = DateTime.UtcNow.AddHours(4);
            order.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }
        #endregion

    }
}
