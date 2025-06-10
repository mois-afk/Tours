namespace Tours.Service.Interface
{
    public interface ITokenService
    {
        public Token GetToken(string email, string role, DateTime? expTime = null);

        public string GetEmailByToken(string token);

        public Token RefreshToken(string refreshToken);
    }
}
