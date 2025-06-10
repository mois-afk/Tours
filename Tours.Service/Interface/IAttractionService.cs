namespace Tours.Service.Interface
{
    using Tours.Models;

    public interface IAttractionService
    {
        public Task<List<Attraction>> GetAllAttractionAsync();

        public Task<List<Attraction>> GetAllAttractionsByCityIdAsync(string cityId);

        public Task<List<Attraction>> GetAllAtractionByPartNameAsync(string partName);

        public Task<bool> AddAttractionAsync(AttractionModel model);

        public Task<bool> UpdateAttractionAsync(AttractionModel model);

        public Task<Attraction> GetAttractionByIdAsync(string id);

        public Task DeleteAttrationByIdAsync(string id);
    }
}
