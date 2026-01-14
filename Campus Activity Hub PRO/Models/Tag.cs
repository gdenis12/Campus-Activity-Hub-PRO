namespace Campus_Activity_Hub_PRO.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    }
}
