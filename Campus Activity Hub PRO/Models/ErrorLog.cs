namespace Campus_Activity_Hub_PRO.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Path { get; set; } = null!;
        public string? UserEmail { get; set; }
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
    }
}
