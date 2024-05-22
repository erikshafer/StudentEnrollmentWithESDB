namespace StudentEnrollment01.Events;

public record StudentEnrolled : Event
{
    public required string CourseName { get; init; }
    public DateTime EnrolledAtUtc { get; init; }
}