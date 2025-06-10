namespace Tours.Models
{
    using Tours;

    public class OrderModel
    {
        public string? Username { get; set; }

        public List<Tour>? TourList { get; set; } = new List<Tour>();

        public double TotalPrice { get; set; }

        public DateTime Date { get; set; }
    }
}
