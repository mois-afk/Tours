namespace Tours.Interface
{
    using Tours.Models;

    public interface ITourRepository
    {
        public Task<string> AddTour(string name, string description, double price, DateTime startDate, DateTime endDate, Dictionary<string, DateTime> attractionDate, string url, bool userGenerated = false);

        public Task<List<Tour>> GetAllTours();

        public Task<List<Tour>> GetAllByNameTours(string namePart);

        public Task<Tour> GetTourById(string id);

        public Task<bool> UpdateTour(TourModel tour);

        public Task<Dictionary<string, DateTime>> GetAttractionDateByTourId(string id);

        public Task<List<string>> GetAllAttractionByTourId(string tourId);

        public Task<List<Tour>> GetAllToursByAllId(List<string> tourIdList);

        public Task DeleteTour(string id);
    }
}
