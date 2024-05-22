namespace StudentEnrollment02.Esdb.Events;

public abstract class Event
{
    public required string StudentId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}