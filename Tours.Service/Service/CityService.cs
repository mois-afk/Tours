namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public Task<List<City>> GetAllCity()
        {
            return _cityRepository.GetAllCity();
        }

        public Task<City> GetCityById(string id)
        {
            return _cityRepository.GetCityById(id);
        }

        public async Task AddCity(CityModel model)
        {
            await _cityRepository.AddCity(model.URL, model.CityName, model.CityDescription);
        }

        public async Task<List<City>> GetAllCityByPartName(string partName)
        {
            return await _cityRepository.GetAllCityByName(partName);
        }

        public async Task DeleteCityByIdAsync(string id)
        {
            await _cityRepository.DeleteCityById(id);
        }

        public async Task UpdateCityAsync(CityModel model)
        {
            await _cityRepository.UpdateCity(model);
        }
    }
}
