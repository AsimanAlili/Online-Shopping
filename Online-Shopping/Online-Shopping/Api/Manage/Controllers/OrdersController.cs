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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Order order = await _context.Orders
                .Include(x => x.AppUser)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckOrderNotFound
            if (order == null)
                return NotFound();
            #endregion
            OrderGetDto orderGetDto = _mapper.Map<OrderGetDto>(order);


            return Ok(orderGetDto);
        }

        #region GetAllPage
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Order> orders = await _context.Orders
                .Include(x => x.AppUser).Include(x => x.Product)
                .Skip((page - 1) * 10).Take(10).ToListAsync();

            OrderListDto orderList = new OrderListDto
            {
                Orders = _mapper.Map<List<OrderItemDto>>(orders),
                TotalCount = await _context.Orders.CountAsync()
            };

            return Ok(orderList);
        }
        #endregion



        [HttpPost("accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            order.Status = Data.Enums.OrderStatus.Accepted;
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpPost("courier/{id}")]
        public async Task<IActionResult> CourierDelivery(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            order.Status = Data.Enums.OrderStatus.CourierDelivery;
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpPost("delivered/{id}")]
        public async Task<IActionResult> Delivered(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            order.Status = Data.Enums.OrderStatus.Delivered;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("rejected/{id}")]
        public async Task<IActionResult> Rejected(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            order.Status = Data.Enums.OrderStatus.Rejected;
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}
