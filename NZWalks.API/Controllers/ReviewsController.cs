using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/reviews
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IReviewRepository reviewRepository;

        public ReviewsController(IMapper mapper, IReviewRepository reviewRepository)
        {
            this.mapper = mapper;
            this.reviewRepository = reviewRepository;
        }

        // CREATE Review
        // POST: /api/reviews
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddReviewRequestDto addReviewRequestDto)
        {
            // Map DTO to Domain Model
            var reviewDomainModel = mapper.Map<Review>(addReviewRequestDto);

            await reviewRepository.CreateAsync(reviewDomainModel);

            // Map Domain Model to DTO
            var reviewDto = mapper.Map<ReviewDto>(reviewDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = reviewDto.Id }, reviewDto);
        }

        // GET: Reviews
        // GET: /api/reviews?filterOn=Comment&filterQuery=great&sortBy=Rating&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var reviewsDomainModel = await reviewRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                    isAscending ?? true, pageNumber, pageSize);

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<ReviewDto>>(reviewsDomainModel));
        }

        // Get Review By Id
        // GET: /api/reviews/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var reviewDomainModel = await reviewRepository.GetByIdAsync(id);

            if (reviewDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<ReviewDto>(reviewDomainModel));
        }

        // Update Review By Id
        // PUT: /api/reviews/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateReviewRequestDto updateReviewRequestDto)
        {
            // Map DTO to Domain Model
            var reviewDomainModel = mapper.Map<Review>(updateReviewRequestDto);

            reviewDomainModel = await reviewRepository.UpdateAsync(id, reviewDomainModel);

            if (reviewDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<ReviewDto>(reviewDomainModel));
        }

        // Delete a Review By Id
        // DELETE: /api/reviews/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedReviewDomainModel = await reviewRepository.DeleteAsync(id);

            if (deletedReviewDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<ReviewDto>(deletedReviewDomainModel));
        }

    }
}
