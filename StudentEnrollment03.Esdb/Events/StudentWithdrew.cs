namespace StudentEnrollment03.Esdb.Events;

public record StudentWithdrew : Event
{
    public required string CourseName { get; init; }
    public DateTime WithdrawnAtUtc { get; init; }
}