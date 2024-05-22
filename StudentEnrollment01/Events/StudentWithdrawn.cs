namespace StudentEnrollment01.Events;

public record StudentWithdrawn : Event
{
    public required string CourseName { get; init; }
    public DateTime WithdrawnAtUtc { get; init; }
}