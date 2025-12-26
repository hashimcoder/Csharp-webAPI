using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // API/Controller for Walks
    // http://localhost:5000/api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //CREATE Walks
        // POST: http://localhost:5000/api/walks

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {

            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
        // Get All Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10


        [HttpGet]
        public async Task<IActionResult> GetAll(
           [FromQuery] string? filterOn, 
           [FromQuery] string? filterQuery,
              [FromQuery] string? sortBy,
              [FromQuery] bool isAscending = true,
              [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 1000
           )
           
        {
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery,sortBy,isAscending, pageNumber, pageSize);
            // Map Domain Model to DTO
            var walkDto = mapper.Map<List<WalkDto>>(walkDomainModel);
            return Ok(walkDto);
        }
        // GET: http://localhost:5000/api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
        }

        // PUT: http://localhost:5000/api/walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {

            // Debug: Log the incoming DifficultyId
            Console.WriteLine($"Received DifficultyId: {updateWalkRequestDto.DifficultyId}");
            Console.WriteLine($"Received RegionId: {updateWalkRequestDto.RegionId}");

            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            // check if walk exists
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);

        }
        // DELETE: http://localhost:5000/api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // check if walk exists
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

    }
}