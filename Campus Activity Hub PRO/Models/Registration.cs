using Campus_Activity_Hub_PRO.Models;

namespace Campus_Activity_Hub_PRO.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }
        public string? Comment { get; set; }

        public string? Feedback { get; set; }
        public DateTime? FeedbackAt { get; set; }
    }
}
