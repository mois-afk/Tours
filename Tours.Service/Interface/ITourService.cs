namespace Tours.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Models;

    public interface ITourService
    {
        public Task<string> AddTourAsync(TourModel model, bool userGenerated = false);

        public Task<Tour> GetTourByIdAsync(string id);

        public Task<List<Tour>> GetAllToursByAllIdAsync(List<string> tourIdList);

        public Task<List<Tour>> GetAllToursAsync();

        public Task<List<Attraction>> GetAllAttractionForTour(string tourId);

        public Task<Dictionary<Attraction, DateTime>> GetAttractionDateForTour(string id);

        public Task<List<Tour>> GetAllTourByPartNameAsync(string partName);

        public Task<bool> UpdateTourAsync(TourModel model);

        public Task<Dictionary<string, DateTime>> AddToMyTour(string tourId);

        public Task DeleteTour(string tourId);
    }
}
