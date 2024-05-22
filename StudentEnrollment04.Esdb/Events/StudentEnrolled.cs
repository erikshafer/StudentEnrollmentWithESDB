namespace StudentEnrollment04.Esdb.Events;

public record StudentEnrolled : Event
{
    public required string CourseName { get; init; }
    public DateTime EnrolledAtUtc { get; init; }
}