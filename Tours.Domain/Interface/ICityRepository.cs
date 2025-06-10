namespace Tours.Interface
{
    using Tours.Models;

    public interface ICityRepository
    {
        public Task<List<City>> GetAllCity();

        public Task<City> GetCityById(string id);

        public Task AddCity(string url, string name, string descriprion);

        public Task<bool> UpdateCity(CityModel model);

        public Task<List<City>> GetAllCityByName(string namePart);

        public Task DeleteCityById(string id);
    }
}
