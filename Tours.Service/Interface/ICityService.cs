namespace Tours.Service.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Models;

    public interface ICityService
    {
        public Task<List<City>> GetAllCity();

        public Task AddCity(CityModel model);

        public Task UpdateCityAsync(CityModel model);

        public Task<List<City>> GetAllCityByPartName(string partName);

        public Task<City> GetCityById(string id);

        public Task DeleteCityByIdAsync(string id);
    }
}
