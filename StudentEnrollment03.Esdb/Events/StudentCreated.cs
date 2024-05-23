namespace StudentEnrollment03.Esdb.Events;

public record StudentCreated : Event
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}