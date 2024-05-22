using StudentEnrollment00.InMemory.Events;

namespace StudentEnrollment00.InMemory;

public class Student
{
    public Guid Id { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public List<string> EnrolledCourses { get; set; } = [];

    public void Apply(Event @event)
    {
        switch (@event)
        {
            case StudentCreated created:
                Apply(created);
                break;
            case StudentUpdated updated:
                Apply(updated);
                break;
            case StudentEnrolled enrolled:
                Apply(enrolled);
                break;
            case StudentUnEnrolled unEnrolled:
                Apply(unEnrolled);
                break;
        }
    }

    private void Apply(StudentCreated @event)
    {
        Id = @event.StudentId;
        FullName = @event.FullName;
        Email = @event.Email;
    }

    private void Apply(StudentUpdated @event)
    {
        Id = @event.StudentId; // Updating the identity, huh? Interesting... 👀
        FullName = @event.FullName;
        Email = @event.Email;
    }

    private void Apply(StudentEnrolled @event)
    {
        if (EnrolledCourses.Contains(@event.CourseName) is false)
            EnrolledCourses.Add(@event.CourseName);
    }

    private void Apply(StudentUnEnrolled @event)
    {
        if (EnrolledCourses.Contains(@event.CourseName))
            EnrolledCourses.Add(@event.CourseName);
    }
}