using AutoMapper;
using Online_Shopping.Helpers;
using Microsoft.AspNetCore.Hosting;
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
    public class SlidersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SlidersController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SliderCreateDto createDto)
        {
            #region CheckOrderExist
            if (await _context.Sliders.AnyAsync(x => x.Order == createDto.Order))
                return Conflict($"Slider Already exist by order {createDto.Order}");
            #endregion
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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/sliders", createDto.File);

                createDto.Photo = filename;
            }
            #endregion

            Slider slider = _mapper.Map<Slider>(createDto);
            slider.CreatedAt = DateTime.UtcNow.AddHours(4);
            slider.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return StatusCode(201, slider.Id);
        }
        #endregion
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSliderNotFound
            if (slider == null)
                return NotFound();
            #endregion
            SliderGetDto sliderGetDto = _mapper.Map<SliderGetDto>(slider);

            return Ok(sliderGetDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Slider> sliders = await _context.Sliders.OrderByDescending(x => x.Order).Skip((page - 1) * 8).Take(8).ToListAsync();

            SliderListDto slidersDto = new SliderListDto
            {
                Sliders = _mapper.Map<List<SliderItemDto>>(sliders),
                TotalCount = await _context.Sliders.CountAsync()
            };

            return Ok(slidersDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Slider> sliders = await _context.Sliders.OrderByDescending(x => x.Order).ToListAsync();
            return Ok(_mapper.Map<List<SliderItemDto>>(sliders));
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] SliderCreateDto editDto)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckOrderExist
            if (await _context.Sliders.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"Slider Already exist by order {editDto.Order}");
            #endregion
            #region CheckSliderNotFound
            if (slider == null)
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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/sliders", editDto.File);
                if (!string.IsNullOrWhiteSpace(slider.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/sliders", slider.Photo);
                }
                slider.Photo = filename;
            }

            #endregion

            slider.Title = editDto.Title;
            slider.Order = editDto.Order;
            slider.Text = editDto.Text;
            slider.RedirectUrl = editDto.RedirectUrl;
            slider.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(slider);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSliderNotFound
            if (slider == null)
                return NotFound();
            #endregion
            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
