using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IReviewRepository
    {
        Task<Review> CreateAsync(Review review);
        Task<List<Review>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAcending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Review?> GetByIdAsync(Guid id);

        Task<Review?> UpdateAsync(Guid id, Review review);

        Task<Review?> DeleteAsync(Guid id);
    }
}
