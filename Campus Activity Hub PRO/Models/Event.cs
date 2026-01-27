using Campus_Activity_Hub_PRO.Models;

namespace Campus_Activity_Hub_PRO.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }

        public string? PosterPath { get; set; }
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string OrganizerId { get; set; } = "";
        public AppUser Organizer { get; set; } = null!;

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    }
}
