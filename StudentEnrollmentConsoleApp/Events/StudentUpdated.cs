namespace StudentEnrollmentConsoleApp.Events;

public class StudentUpdated : Event
{
    public required string StudentId { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    
    public override string StreamId => StudentId;
}