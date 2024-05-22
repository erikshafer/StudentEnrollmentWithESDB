namespace StudentEnrollment02.Esdb.Events;

public class StudentWithdrawn : Event
{
    public required string CourseName { get; init; }
}