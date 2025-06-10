namespace Tours.Service.Interface
{
    using Tours.Models;

    public interface IReviewService
    {
        public Task<List<ReviewModel>> GetAllReviews();

        public Task AddReviewAsync(string text, string id, string username);
    }
}
