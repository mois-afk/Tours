namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        private readonly IAttractionRepository _attractionRepository;

        private readonly IRedisService _redisService;

        public TourService(ITourRepository tourRepository, IAttractionRepository attractionRepository, IRedisService redisService)
        {
            _tourRepository = tourRepository;
            _attractionRepository = attractionRepository;
            _redisService = redisService;
        }

        public async Task<List<Tour>> GetAllToursAsync()
        {
            return await _tourRepository.GetAllTours();
        }

        public async Task<Tour> GetTourByIdAsync(string id)
        {
            return await _tourRepository.GetTourById(id);
        }

        public async Task<List<Tour>> GetAllToursByAllIdAsync(List<string> tourIdList)
        {
            return await _tourRepository.GetAllToursByAllId(tourIdList);
        }

        public async Task<List<Attraction>> GetAllAttractionForTour(string tourId)
        {
            var attractionIds = await _tourRepository.GetAllAttractionByTourId(tourId);
            var attractions = new List<Attraction>();
            foreach (var attractionId in attractionIds)
            {
                var attraction = await _attractionRepository.GetAttractionById(attractionId);
                attractions.Add(attraction);
            }

            return attractions;
        }

        public async Task<Dictionary<Attraction, DateTime>> GetAttractionDateForTour(string id)
        {
            var attractionDate = new Dictionary<Attraction, DateTime>();

            var attractionDateById = await _tourRepository.GetAttractionDateByTourId(id);

            foreach (var kvp in attractionDateById)
            {
                var attraction = await _attractionRepository.GetAttractionById(kvp.Key);
                if (attraction != null)
                {
                    attractionDate.Add(attraction, kvp.Value);
                }
            }

            return attractionDate;
        }

        public async Task<string> AddTourAsync(TourModel model, bool userGenerated = false)
        {

            var id = await _tourRepository.AddTour(model.TourName, model.TourDescription, model.TourPrice, model.StartDate, model.EndDate, model.AttractionDate, model.URL, userGenerated);

            return id;
        }

        public async Task<bool> UpdateTourAsync(TourModel model)
        {
            return await _tourRepository.UpdateTour(model);
        }

        public async Task<List<Tour>> GetAllTourByPartNameAsync(string partName)
        {
            return await _tourRepository.GetAllByNameTours(partName);
        }

        public async Task<Dictionary<string, DateTime>> AddToMyTour(string tourId)
        {
            var attractionDate = await _tourRepository.GetAttractionDateByTourId(tourId);

            Dictionary<string, DateTime> myTourList = _redisService.Get<Dictionary<string, DateTime>>("MyTourList");

            if (myTourList == null)
            {
                myTourList = new Dictionary<string, DateTime>();
            }

            DateTime? lastTourDate = myTourList.Values.Count > 0 ? (DateTime?)myTourList.Values.Max() : null;

            DateTime firstNewTourDate = attractionDate.Values.Min();

            if (lastTourDate.HasValue && firstNewTourDate < lastTourDate.Value)
            {
                return myTourList;
            }

            foreach (var attraction in attractionDate)
            {
                myTourList[attraction.Key] = attraction.Value;
            }

            return myTourList;
        }

        public async Task DeleteTour(string tourId)
        {
            await _tourRepository.DeleteTour(tourId);
        }
    }
}
