namespace StudentEnrollment02.Esdb.Events;

public class StudentWithdrew : Event
{
    public required string CourseName { get; init; }
}