namespace StudentEnrollment02.Esdb.Events;

public class StudentEnrolled : Event
{
    public required string CourseName { get; init; }
}