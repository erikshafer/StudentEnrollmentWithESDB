namespace StudentEnrollment02.Esdb.Events;

public record StudentEmailChanged : Event
{
    public required string Email { get; init; }
    public DateTime ChangedAtUtc { get; init; }
}