namespace Tours.Interface
{
    public interface IEmailRepository
    {
        public Task SaveEmail(Email email);
    }
}
