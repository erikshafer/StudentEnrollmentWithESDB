namespace StudentEnrollment04.Esdb.Events;

public record StudentWithdrawn : Event
{
    public required string CourseName { get; init; }
    public DateTime WithdrawnAtUtc { get; init; }
}