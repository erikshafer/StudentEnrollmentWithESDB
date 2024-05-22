namespace StudentEnrollment01.InMemory.Events;

public class StudentEnrolled : Event
{
    public required Guid StudentId { get; init; }
    public required string CourseName { get; init; }

    public override Guid StreamId => StudentId;
}