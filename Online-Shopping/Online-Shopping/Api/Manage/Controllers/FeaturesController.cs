using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FeaturesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(FeatureCreateDto createDto)
        {
            #region CheckFeatureExist
            if (await _context.Features.AnyAsync(x => x.Title.ToUpper() == createDto.Title.ToUpper().Trim()))
                return Conflict($"Feature already exist by name {createDto.Title}");
            #endregion
            #region CheckOrderExist
            if (await _context.Features.AnyAsync(x => x.Order == createDto.Order))
                return Conflict($"Feature Already exist by order {createDto.Order}");
            #endregion



            Feature feature = _mapper.Map<Feature>(createDto);
            feature.ModifiedAt = DateTime.UtcNow.AddHours(4);
            feature.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();

            return StatusCode(201, feature.Id);
        }
        #endregion
        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckFeatureNotFound
            if (feature == null)
                return NotFound();
            #endregion
            FeatureGetDto getDto = _mapper.Map<FeatureGetDto>(feature);

            return Ok(getDto);
        }
        #endregion

        #region GetAllPagination
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Feature> features = await _context.Features.Skip((page - 1) * 5).Take(5).OrderBy(x=>x.Order).ToListAsync();
            FeatureListDto listDto = new FeatureListDto
            {
                Features = _mapper.Map<List<FeatureItemDto>>(features),
                TotalCount = await _context.Features.CountAsync()
            };
            return Ok(listDto);
        }
        #endregion

        #region GetAll
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Feature> features = await _context.Features.ToListAsync();
            var map = _mapper.Map<List<FeatureItemDto>>(features);

            return Ok(map);
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, FeatureCreateDto editDto)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);

            #region CheckSubFeatureExist
            if (await _context.Features.AnyAsync(x => x.Title.ToUpper() == editDto.Title.ToUpper().Trim() && x.Id != id))
                return Conflict($"Feature already exist by name {editDto.Title}");
            #endregion
            #region CheckOrderExist
            if (await _context.Features.AnyAsync(x => x.Order == editDto.Order && x.Id != id))
                return Conflict($"Feature Already exist by order {editDto.Order}");
            #endregion
            #region CheckFeatureNotFound
            if (feature == null)
                return NotFound();
            #endregion

            feature.Title = editDto.Title;
            feature.SubTitle = editDto.SubTitle;
            feature.Icon = editDto.Icon;
            feature.Order = editDto.Order;
            feature.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok(feature);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckFeatureNotFound
            if (feature == null)
                return NotFound();
            #endregion

            _context.Features.Remove(feature);
            _context.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
