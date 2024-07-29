using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repository;
using System.Globalization;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _loggerRegion;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> loggerRegion)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _loggerRegion = loggerRegion;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionRepository.GetAllAsync();
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

            _loggerRegion.LogInformation($"Finnished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)} ");
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidatedModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionDto AddRegionDtoRequest)
        {
            var regionDomain = _mapper.Map<Region>(AddRegionDtoRequest);
            regionDomain = await _regionRepository.CreateAsync(regionDomain);
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return CreatedAtAction(nameof(GetById), new { Id = regionDomain.Id }, regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidatedModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto UpdateRegionDtoRequest)
        {
            var regionDomain = _mapper.Map<Region>(UpdateRegionDtoRequest);
            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }
}
