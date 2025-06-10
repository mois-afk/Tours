namespace Tours.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderRepository
    {
       public Task<List<Order>> GetAllByUserId(string id);

       public Task<List<string>> GetAllTourByUserId(string id);

       public Task AddOrder(string userId, List<Tour> tours, DateTime data, double totalPrice);

       public Task DeleteOrder(string userId);
    }
}
