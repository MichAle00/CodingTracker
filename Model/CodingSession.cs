namespace CodingTracker.Model;

internal class CodingSession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan Duration { get; set; }
}
