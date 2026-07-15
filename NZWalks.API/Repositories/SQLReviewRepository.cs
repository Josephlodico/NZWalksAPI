using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLReviewRepository : IReviewRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLReviewRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await dbContext.Reviews.AddAsync(review);
            await dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> DeleteAsync(Guid id)
        {
            var existingReview = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            if (existingReview == null)
            {
                return null;
            }
            dbContext.Reviews.Remove(existingReview);
            await dbContext.SaveChangesAsync();
            return existingReview;
        }
        public async Task<List<Review>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
           string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var reviews = dbContext.Reviews.Include("Walk").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Comment", StringComparison.OrdinalIgnoreCase))
                {
                    reviews = reviews.Where(x => x.Comment.Contains(filterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Rating", StringComparison.OrdinalIgnoreCase))
                {
                    reviews = isAscending ? reviews.OrderBy(x => x.Rating) : reviews.OrderByDescending(x => x.Rating);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await reviews.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await dbContext.Reviews.
                Include("Walk").
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Review?> GetbyIdAsync(Guid id, Review review)
        {
            throw new NotImplementedException();
        }

        public async Task<Review?> UpdateAsync(Guid id, Review review)
        {
            var existingReview = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if (existingReview == null)
            {
                return null;
            }

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.WalkId = review.WalkId;

            await dbContext.SaveChangesAsync();

            return existingReview;
        }
    }
}
