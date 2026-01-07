using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // API/controller for Regions
    //http://localhost:5000/api/regions
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase

    {

        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // Controller methods will go here
        // Action methods for handling HTTP requests related to Regions

        // GET: ALL REGIONS
        //GET: http://localhost:PORTnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {    // Get Data from Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Using AutoMapper to map Domain Models to DTOs
            // Return DTOs to client
            var RegionDto = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(RegionDto);
        }
        // Get region by ID
        // GET: http://localhost:PORTnumber/api/regions/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader, Writer")]

        public async Task<IActionResult> GetById(Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            // Get region  domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map/Convert region Domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            // Return DTO to client
            return Ok(regionDto);


        }



        // Post: Create a new region
        // POST: http://localhost:PORTnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            // Call Repository to persist the data
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            // Convert the Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            // Return the DTO to client
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        // UPDATE Region
        // PUT: http://localhost:PORTnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            // Check if region exists
            await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert the Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            // Return the DTO to client
            return Ok(regionDto);

        }

        // DELETE Region
        // DELETE: http://localhost:PORTnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Check if region exists
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert the Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            // Return the DTO to client
            return Ok(regionDto);
        }

    }
}