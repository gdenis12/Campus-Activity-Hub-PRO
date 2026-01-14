using Microsoft.Extensions.Logging;

namespace Campus_Activity_Hub_PRO.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? IconCss { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
