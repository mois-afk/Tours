namespace Tours
{
    public class Ticket
    {
        public string Url { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTimeOffset DepartureDate { get; set; }

        public DateTimeOffset ArrivalDate { get; set; }

        public string TransportType { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }
    }
}
