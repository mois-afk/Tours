namespace Tours
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Order
    {
        public Order(string userId, List<Tour> tours, DateTime dateOrder, double totalPrice)
        {
            UserId = userId;
            Tours = tours;
            DateOrder = dateOrder;
            TotalPrice = totalPrice;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public double TotalPrice { get; set; }

        public List<Tour> Tours { get; set; }

        public DateTime DateOrder { get; set; }
    }
}
