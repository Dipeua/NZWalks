using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walksRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walksRepository, IMapper mapper)
        {
            _walksRepository = walksRepository;
            _mapper = mapper;
        }

        // api/walks/filterOn=Name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDomain = await _walksRepository.GetAllAsync(filterOn, filterQuery);
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);
        }


        [HttpPost]
        [ValidatedModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto)
        {
            var walkDomain = _mapper.Map<Walk>(addWalkDto);
            walkDomain = await _walksRepository.CreateAsync(walkDomain);
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await _walksRepository.GetByIdAsync(id);
            if (walkDomain == null) { return NotFound(); }

            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidatedModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var walkDomain = _mapper.Map<Walk>(updateWalkDto);

            walkDomain = await _walksRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null) { return NotFound(); }

            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await _walksRepository.DeleteAsync(id);
            if(walkDomain == null) { return NotFound(); }
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

    }
}
