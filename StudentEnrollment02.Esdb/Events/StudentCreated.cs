namespace StudentEnrollment02.Esdb.Events;

public class StudentCreated : Event
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required DateTime DateOfBirth { get; init; }
}