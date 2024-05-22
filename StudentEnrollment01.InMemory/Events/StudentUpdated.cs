namespace StudentEnrollment01.InMemory.Events;

public class StudentUpdated : Event
{
    public required Guid StudentId { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }

    public override Guid StreamId => StudentId;
}