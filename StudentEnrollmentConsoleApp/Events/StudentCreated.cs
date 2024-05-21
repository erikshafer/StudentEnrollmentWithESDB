namespace StudentEnrollmentConsoleApp.Events;

public class StudentCreated : Event
{
    public required string StudentId { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required DateTime DateOfBirth { get; init; }

    public override string StreamId => StudentId;
}