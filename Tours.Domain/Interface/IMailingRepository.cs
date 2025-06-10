namespace Tours.Interface
{
    public interface IMailingRepository
    {
        public Task<Mailing> GetMailingById(string id);

        public Task AddMailing(Mailing mailing);

        public Task<List<Mailing>> GetAllMailing();

        public Task<List<string>> GetEmailList(string id);

        public Task<bool> AddToEmailList(string email, string id);

        public Task<bool> DeleteFromEmailList(string email, string id);

        public Task<bool> DeleteMailing(string id);
    }
}
