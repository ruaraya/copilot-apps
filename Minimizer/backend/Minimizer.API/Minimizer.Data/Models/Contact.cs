namespace Minimizer.Entities.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public int LeadId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public Lead Lead { get; set; }
    }
}
