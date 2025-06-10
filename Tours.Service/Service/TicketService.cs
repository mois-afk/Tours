namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Tours.Service.Interface;

    public class TicketService : ITicketService
    {
        private readonly string _ticketsFilePath = "tickets.json";
        private readonly IAttractionService _attractionService;
        private readonly ICityService _cityService;
        private readonly ITourService _tourService;

        public TicketService(IAttractionService attractionService, ICityService cityService, ITourService tourService)
        {
            _attractionService = attractionService;
            _cityService = cityService;
            _tourService = tourService;
        }

        public async Task<List<Ticket>> GetTicketsForTourAsync(string tourId)
        {
            var tour = await _tourService.GetTourByIdAsync(tourId);
            var allTickets = new List<Ticket>();

            // Сортируем достопримечательности по дате посещения
            var sortedAttractions = tour.AttractionDate.OrderBy(ad => ad.Value).ToList();

            for (int i = 0; i < sortedAttractions.Count - 1; i++)
            {
                var currentAttractionId = sortedAttractions[i].Key;
                var nextAttractionId = sortedAttractions[i + 1].Key;

                var currentAttraction = await _attractionService.GetAttractionByIdAsync(currentAttractionId);
                var nextAttraction = await _attractionService.GetAttractionByIdAsync(nextAttractionId);

                var currentCity = await _cityService.GetCityById(currentAttraction.CityId);
                var nextCity = await _cityService.GetCityById(nextAttraction.CityId);

                string fromCity = currentCity.CityName;
                string toCity = nextCity.CityName;

                // Получаем дату прибытия для следующего города (дата следующей достопримечательности)
                DateTime arrivalDate = sortedAttractions[i + 1].Value;

                // Проверка, что города не совпадают
                if (fromCity != toCity)
                {
                    // Получаем билеты на поезд и на самолет с учетом даты прибытия
                    var trainTickets = await GetTrainTicketLinksAsync(fromCity, toCity, arrivalDate);
                    var flightTickets = await GetFlightTicketLinksAsync(fromCity, toCity, arrivalDate);

                    allTickets.AddRange(trainTickets);
                    allTickets.AddRange(flightTickets);
                }
            }

            // Добавляем билеты из последнего города в первый
            if (sortedAttractions.Count > 1)
            {
                var firstAttractionId = sortedAttractions.First().Key;
                var lastAttractionId = sortedAttractions.Last().Key;

                var firstAttraction = await _attractionService.GetAttractionByIdAsync(firstAttractionId);
                var lastAttraction = await _attractionService.GetAttractionByIdAsync(lastAttractionId);

                var firstCity = await _cityService.GetCityById(firstAttraction.CityId);
                var lastCity = await _cityService.GetCityById(lastAttraction.CityId);

                string fromCity = lastCity.CityName;
                string toCity = firstCity.CityName;

                // Получаем дату прибытия для первого города (дата первой достопримечательности)
                DateTime arrivalDate = sortedAttractions.Last().Value;

                if (fromCity != toCity)
                {
                    var trainTicketsBack = await GetTrainTicketLinksAsync(fromCity, toCity, arrivalDate);
                    var flightTicketsBack = await GetFlightTicketLinksAsync(fromCity, toCity, arrivalDate);

                    allTickets.AddRange(trainTicketsBack);
                    allTickets.AddRange(flightTicketsBack);
                }
            }

            return allTickets;
        }

        // Получение билетов на поезд
        private async Task<List<Ticket>> GetTrainTicketLinksAsync(string fromCity, string toCity, DateTime arrivalDate)
        {
            // Загружаем билеты из файла
            var tickets = await LoadTicketsFromFileAsync();

            // Фильтруем билеты на поезд с учетом даты прибытия
            var trainTickets = tickets
                .Where(ticket => ticket.TransportType.Equals("Поезд", StringComparison.OrdinalIgnoreCase) &&
                                 ticket.FromCity.Equals(fromCity, StringComparison.OrdinalIgnoreCase) &&
                                 ticket.ToCity.Equals(toCity, StringComparison.OrdinalIgnoreCase) &&
                                 ticket.ArrivalDate.Date == arrivalDate.Date) // Учитываем дату прибытия
                .ToList();

            // Проверяем время прибытия и обновляем статус
            foreach (var ticket in trainTickets)
            {
                var timeDifference = arrivalDate - ticket.ArrivalDate;

                if (timeDifference.TotalMinutes >= 60)
                {
                    ticket.Status = "success"; // Успевает 100%
                }
                else if (timeDifference.TotalMinutes >= 30)
                {
                    ticket.Status = "warning"; // Успевает 50/50
                }
                else
                {
                    ticket.Status = "danger"; // Не успевает
                }
            }

            return trainTickets;
        }

        private async Task<List<Ticket>> GetFlightTicketLinksAsync(string fromCity, string toCity, DateTime arrivalDate)
        {
            // Загружаем билеты из файла
            var tickets = await LoadTicketsFromFileAsync();

            // Фильтруем билеты на самолёт с учетом даты прибытия
            var flightTickets = tickets
                .Where(ticket => ticket.TransportType.Equals("Самолёт", StringComparison.OrdinalIgnoreCase) &&
                                 ticket.FromCity.Equals(fromCity, StringComparison.OrdinalIgnoreCase) &&
                                 ticket.ToCity.Equals(toCity, StringComparison.OrdinalIgnoreCase) &&
                                 ticket.ArrivalDate.Date == arrivalDate.Date) // Учитываем дату прибытия
                .ToList();

            // Проверяем время прибытия и обновляем статус
            foreach (var ticket in flightTickets)
            {
                var timeDifference = arrivalDate - ticket.ArrivalDate;

                if (timeDifference.TotalMinutes >= 60)
                {
                    ticket.Status = "success"; // Успевает 100%
                }
                else if (timeDifference.TotalMinutes >= 30)
                {
                    ticket.Status = "warning"; // Успевает 50/50
                }
                else
                {
                    ticket.Status = "danger"; // Не успевает
                }
            }

            return flightTickets;
        }

        private async Task<List<Ticket>> LoadTicketsFromFileAsync()
        {
            try
            {
                var json = await File.ReadAllTextAsync(_ticketsFilePath);
                var ticketData = JsonConvert.DeserializeObject<TicketResponse>(json);
                return ticketData.Tickets;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }

        private class TicketResponse
        {
            public List<Ticket> Tickets { get; set; }
        }

        private class ApiTicketResponse
        {
            public string Url { get; set; }

            public string Price { get; set; }
        }

        private class ApiFlightResponse
        {
            public List<ApiFlightData> Data { get; set; }
        }

        private class ApiFlightData
        {
            public string Url { get; set; }

            public string Price { get; set; }
        }
    }
}
