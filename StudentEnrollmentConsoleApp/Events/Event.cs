namespace StudentEnrollmentConsoleApp.Events;

public abstract class Event
{
    public abstract string StreamId { get; }
    
    public DateTime CreatedAtUtc { get; set; }
}