namespace Tours.Service.Interface
{
    public interface ITicketService
    {
        public Task<List<Ticket>> GetTicketsForTourAsync(string tourId);
    }
}
