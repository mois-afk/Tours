namespace Tours.Interface
{
    using System;

    public interface IReviewRepository
    {
        public Task<List<Review>> GetAllReview();

        public Task<List<Review>> GetAllReviewsByUsername(string username);

        public Task AddReview(Review review);
    }
}
