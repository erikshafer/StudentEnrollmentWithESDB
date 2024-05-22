namespace StudentEnrollment00.Events;

public class StudentEnrolled : Event
{
    public required Guid StudentId { get; init; }
    public required string CourseName { get; init; }

    public override Guid StreamId => StudentId;
}