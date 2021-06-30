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
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckContactNotFound
            if (contact == null)
                return NotFound();
            #endregion
            ContactGetDto getDto = _mapper.Map<ContactGetDto>(contact);

            return Ok(getDto);
        }
        #endregion
    }
}
