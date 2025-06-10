namespace Tours.Service
{
    using Tours.Service.Interface;

    public class AdminService : IAdminService
    {
        private readonly ITourService _tourService;

        private readonly ICityService _cityService;

        private readonly IAttractionService _attractionService;

        private readonly IUserService _userService;

        public AdminService(ITourService tourService, ICityService cityService, IAttractionService attractionService, IUserService userService)
        {
            _tourService = tourService;
            _cityService = cityService;
            _attractionService = attractionService;
            _userService = userService;
        }

        public async Task DeleteUser(string userId)
        {
            await _userService.DeleteUser(userId);
        }

        public async Task DeleteTour(string tourId)
        {
            await _tourService.DeleteTour(tourId);
        }

        public async Task DeleteCity(string cityId)
        {
           await _cityService.DeleteCityByIdAsync(cityId);
           var attList = await _attractionService.GetAllAttractionsByCityIdAsync(cityId);
           foreach (var att in attList)
           {
              await _attractionService.DeleteAttrationByIdAsync(att.AttractionId);
           }
        }

        public async Task DeleteAttractionAsync(string attractionId)
        {
            await _attractionService.DeleteAttrationByIdAsync(attractionId);
        }
    }
}
