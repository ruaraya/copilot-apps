namespace Minimizer.Common.Models.Contacts
{
    public class UpsertContactRequest
    {
        public int LeadId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}