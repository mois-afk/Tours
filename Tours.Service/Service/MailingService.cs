namespace Tours.Service
{
    using System;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class MailingService : IMailingService
    {
        private readonly IEmailService _emailService;
        private readonly IMailingRepository _mailingRepository;

        public MailingService(IEmailService emailService, IMailingRepository mailingRepository)
        {
            _emailService = emailService;
            _mailingRepository = mailingRepository;
        }

        public async Task<List<Mailing>> GetAllMailing()
        {
            return await _mailingRepository.GetAllMailing();
        }

        public async Task<bool> AddToEmailList(string email, string id)
        {
            return await _mailingRepository.AddToEmailList(email, id);
        }

        public async Task<bool> DeleteFromEmailList(string email, string id)
        {
            return await _mailingRepository.DeleteFromEmailList(email, id);
        }

        public async Task<List<string>> GetAllEmailList(string id)
        {
            return await _mailingRepository.GetEmailList(id);
        }

        public async Task<bool> SendMailing(string id)
        {
            var mailing = await _mailingRepository.GetMailingById(id);

            var emailList = await GetAllEmailList(id);

            foreach (var email in emailList)
            {
                if (!_emailService.SendEmail(email, mailing.Subject, mailing.Body))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> AddMailing(MailingModel model)
        {
            if (model.Subject == null || model.Body == null)
            {
                return false;
            }

            Mailing mailing = new Mailing(model.Subject, model.Body, DateTime.Now);
            await _mailingRepository.AddMailing(mailing);
            return true;
        }

        public async Task<bool> DeleteMailing(string id)
        {
            return await _mailingRepository.DeleteMailing(id);
        }
    }
}
