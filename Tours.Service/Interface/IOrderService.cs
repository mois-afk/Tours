namespace Tours.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Tours.Models;

    public interface IOrderService
    {
        public void AddTourToOrder(string tourId);

        public double CalculateTotalPrice(List<Tour> tours);

        public Task<OrderModel> CreateOrder(string username);

        public Task AddOrderAsync(OrderModel model);

        public Task<OrderModel> BuyMyTour(TourModel model, string username);

        public Task DeleteOrder(string id);

        public Task<List<OrderModel>> GetAllOrdersByEmail(string email);

        public void RemoveTourFromOrder(string tourId);
    }
}
