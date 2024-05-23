namespace StudentEnrollment03.Esdb.Events;

public record StudentEnrolled : Event
{
    public required string CourseName { get; init; }
    public required string InstructorName { get; init; }
    public DateTime EnrolledAtUtc { get; init; }
}