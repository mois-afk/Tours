namespace Tours.Models
{
    public class MailingModel
    {
        public string? MailingId { get; set; }

        public List<string>? EmailList { get; set; } = new List<string>();

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public DateTime? Date { get; set; }
    }
}
