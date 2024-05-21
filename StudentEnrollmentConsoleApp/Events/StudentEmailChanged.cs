namespace StudentEnrollmentConsoleApp.Events;

public record StudentEmailChanged : Event
{
    public required string Email { get; init; }
    public DateTime ChangedAtUtc { get; init; }
}