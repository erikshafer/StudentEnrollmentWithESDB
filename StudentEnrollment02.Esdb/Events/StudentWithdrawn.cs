namespace StudentEnrollment02.Esdb.Events;

public record StudentWithdrawn : Event
{
    public required string CourseName { get; init; }
    public DateTime WithdrawnAtUtc { get; init; }
}