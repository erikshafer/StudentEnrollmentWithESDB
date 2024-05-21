namespace StudentEnrollmentConsoleApp.Events;

public class StudentEnrolled : Event
{
    public required string StudentId { get; init; }
    public required string CourseName { get; init; }
    
    public override string StreamId => StudentId;
}