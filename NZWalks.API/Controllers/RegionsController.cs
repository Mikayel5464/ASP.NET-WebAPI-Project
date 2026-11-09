using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data from Database - Domain Models
            var regionsDomain = await _regionRepository.GetAllAsync();
            
            // Map Domain Models to DTOs
            var regionsDtoList = _mapper.Map<List<RegionDTO>>(regionsDomain);
            
            // Return DTOs
            return Ok(regionsDtoList);
        }
        
        // GET Single Region By ID
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetSingleRegionById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = await _regionRepository.GetSingleRegionByIdAsync(id);
            // var region = _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            
            if (regionDomain == null)
            {
                return NotFound();
            }
            
            // Map Region Domain Model to Region DTO
            var regionDto = _mapper.Map<RegionDTO>(regionDomain);

            return Ok(regionDto);
        }
        
        // POST To Create New Region
        // POST: https://localhost:portnumber/api/region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO regionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(regionRequestDto);

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            
            // Map Domain Model back to DTO
            var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetSingleRegionById), new { id = regionDomainModel.Id }, regionDto);
        }
        
        // Update a Region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
        
        // Delete a Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            
            // Optional: We can return a deleted Region back
            // Map Domain Model to DTO
            var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}