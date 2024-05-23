namespace StudentEnrollment02.Esdb.Events;

public class StudentUpdated : Event
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
}