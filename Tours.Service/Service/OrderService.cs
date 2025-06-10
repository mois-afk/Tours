namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ITourRepository _tourRepository;

        private readonly IUserRepository _userRepository;

        private readonly IRedisService _redisService;

        public OrderService(IOrderRepository orderRepository, ITourRepository tourRepository, IUserRepository userRepository, IRedisService redisService)
        {
            _orderRepository = orderRepository;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _redisService = redisService;
        }

        public void AddTourToOrder(string tourId)
        {
            List<string> tourIdList = _redisService.Get<List<string>>("TourIdList");

            if (tourIdList == null)
            {
                tourIdList = new List<string>();
            }

            tourIdList.Add(tourId);

            _redisService.Set("TourIdList", tourIdList);
        }

        public double CalculateTotalPrice(List<Tour> tours)
        {
            double totalPrice = 0;
            foreach (Tour tour in tours)
            {
                totalPrice += tour.TourPrice;
            }

            return totalPrice;
        }

        public async Task<OrderModel> CreateOrder(string username)
        {
            var date = DateTime.Now;
            var tourIdList = _redisService.Get<List<string>>("TourIdList");
            List<Tour> tourList = await _tourRepository.GetAllToursByAllId(tourIdList);
            var totalPrice = CalculateTotalPrice(tourList);
            return new OrderModel
            {
                Username = username,
                TourList = tourList,
                TotalPrice = totalPrice,
                Date = date,
            };
        }

        public async Task AddOrderAsync(OrderModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);
            var tourIdList = _redisService.Get<List<string>>("TourIdList");
            foreach (var tourId in tourIdList)
            {
                var tour = await _tourRepository.GetTourById(tourId);
                model.TourList.Add(tour);
            }

            await _orderRepository.AddOrder(user.UserId, model.TourList, model.Date, model.TotalPrice);
            _redisService.Remove("TourIdList");
        }

        public async Task<OrderModel> BuyMyTour(TourModel model, string username)
        {
            model.AttractionDate = _redisService.Get<Dictionary<string, DateTime>>("MyTourList");
            model.EndDate = model.AttractionDate.Values.Max();
            Tour tour = new Tour(model.TourName, model.TourDescription, model.TourPrice, model.StartDate, model.EndDate, model.AttractionDate, model.URL);
            List<Tour> listTour = new List<Tour>();
            listTour.Add(tour);
            return new OrderModel
            {
                Username = username,
                TourList = listTour,
                Date = DateTime.Now,
                TotalPrice = model.TourPrice,
            };
        }

        public void RemoveTourFromOrder(string tourId)
        {
            List<string> tourIdList = _redisService.Get<List<string>>("TourIdList");

            if (tourIdList != null)
            {
                tourIdList.Remove(tourId);
                _redisService.Set("TourIdList", tourIdList);
            }
        }

        public async Task<List<OrderModel>> GetAllOrdersByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            List<OrderModel> orderModelList = new List<OrderModel>();
            var orders = await _orderRepository.GetAllByUserId(user.UserId);
            if (orders != null || !orders.Any())
            {
                foreach (var order in orders)
                {
                    var totalPrice = CalculateTotalPrice(order.Tours);
                    OrderModel orderModel = new OrderModel
                    {
                        Username = email,
                        TourList = order.Tours,
                        TotalPrice = totalPrice,
                        Date = order.DateOrder,
                    };
                    orderModelList.Add(orderModel);
                }
            }

            return orderModelList != null ? orderModelList : new List<OrderModel>();
        }

        public Task DeleteOrder(string id)
        {
            return _orderRepository.DeleteOrder(id);
        }
    }
}
