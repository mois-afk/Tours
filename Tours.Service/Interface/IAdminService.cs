namespace Tours.Service.Interface
{
    public interface IAdminService
    {
        public Task DeleteUser(string userId);

        public Task DeleteCity(string cityId);

        public Task DeleteAttractionAsync(string attractionId);

        public Task DeleteTour(string tourId);
    }
}
