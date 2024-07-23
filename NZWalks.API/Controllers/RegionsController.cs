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

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain Model 
            var regionsDomain = await _regionRepository.GetAllAsync();
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            // Return DTOs to the client
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
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
