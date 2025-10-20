namespace EventEase
{
    public class Attendance
    {
        public int EventId { get; set; }      // Which event
        public string Username { get; set; } = string.Empty; // Who is attending
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
