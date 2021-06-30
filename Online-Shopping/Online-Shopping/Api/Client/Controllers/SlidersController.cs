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
    public class SlidersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SlidersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Slider> sliders = await _context.Sliders.OrderByDescending(x => x.Order).Take(3).ToListAsync();
            return Ok(_mapper.Map<List<SliderItemDto>>(sliders));
        }
        #endregion
    }
}
