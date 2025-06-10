namespace PIS.Memory
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours;
    using Tours.Interface;
    using Tours.Models;

    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _orderCollection = database.GetCollection<Order>("Orders");
        }

        public async Task<List<Order>> GetAllByUserId(string id)
        {
            return await _orderCollection.Find(order => order.UserId == id).ToListAsync();
        }

        public async Task<List<string>> GetAllTourByUserId(string id)
        {
            var orders = await _orderCollection.Find(order => order.UserId == id).ToListAsync();
            var tourIds = new List<string>();

            foreach (var order in orders)
            {
                foreach (var tour in order.Tours)
                {
                    tourIds.Add(tour.TourId);
                }
            }

            return tourIds;
        }


        public async Task AddOrder(string userId, List<Tour> tours, DateTime date, double totalPrice)
        {
            var order = new Order(userId, tours, date, totalPrice);
            await _orderCollection.InsertOneAsync(order);
        }

        public async Task DeleteOrder(string id)
        {
            await _orderCollection.DeleteOneAsync(order => order.OrderId == id);
        }
    }

}