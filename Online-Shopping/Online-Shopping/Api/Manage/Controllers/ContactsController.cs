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
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(ContactCreateDto createDto)
        {
            Contact contact = _mapper.Map<Contact>(createDto);
            contact.ModifiedAt = DateTime.UtcNow.AddHours(4);
            contact.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return StatusCode(201, contact.Id);
        }
        #endregion
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

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, ContactCreateDto editDto)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckContactNotFound
            if (contact == null)
                return NotFound();
            #endregion

            contact.Address = editDto.Address;
            contact.Phone = editDto.Phone;
            contact.Support = editDto.Support;
            contact.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(contact);
        }
        #endregion
    }
}
