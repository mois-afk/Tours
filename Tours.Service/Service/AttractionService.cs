namespace Tours.Service
{
    using System.Threading.Tasks;
    using Tours;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepository _attractionRepository;

        public AttractionService(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        public async Task<List<Attraction>> GetAllAttractionAsync()
        {
            return await _attractionRepository.GetAllAttractions();
        }

        public async Task<List<Attraction>> GetAllAttractionsByCityIdAsync(string cityId)
        {
            return await _attractionRepository.GetAllAttractionsByCityId(cityId);
        }

        public async Task<List<Attraction>> GetAllAtractionByPartNameAsync(string partName)
        {
            return await _attractionRepository.GetAllAttractionsByName(partName);
        }

        public async Task<bool> AddAttractionAsync(AttractionModel model)
        {
            if (model.AttractionPhotoUrl == null || model.AttractionDescription == null || model.AttractionName == null)
            {
                return false;
            }

            await _attractionRepository.AddAttraction(model.AttractionName, model.AttractionDescription, model.AttractionPhotoUrl, model.CityId);
            return true;
        }

        public async Task<bool> UpdateAttractionAsync(AttractionModel model)
        {
            if (model.AttractionPhotoUrl == null || model.AttractionDescription == null || model.AttractionName == null)
            {
                return false;
            }

            return await _attractionRepository.UpdateAttraction(model);
        }

        public async Task<Attraction> GetAttractionByIdAsync(string id)
        {
            return await _attractionRepository.GetAttractionById(id);
        }

        public async Task DeleteAttrationByIdAsync(string id)
        {
            await _attractionRepository.DeleteAttractionById(id);
        }
    }
}
