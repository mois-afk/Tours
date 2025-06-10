namespace Tours.Service.Interface
{
    using Tours.Models;

    public interface IMailingService
    {
        public Task<List<Mailing>> GetAllMailing();

        public Task<bool> AddToEmailList(string email, string id);

        public Task<bool> DeleteFromEmailList(string email, string id);

        public Task<List<string>> GetAllEmailList(string id);

        public Task<bool> SendMailing(string id);

        public Task<bool> AddMailing(MailingModel model);

        public Task<bool> DeleteMailing(string id);
    }
}
