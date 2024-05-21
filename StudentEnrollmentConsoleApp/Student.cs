using StudentEnrollmentConsoleApp.Events;

namespace StudentEnrollmentConsoleApp;

public class Student
{
    public string Id { get; set; } = default!;
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
            case StudentEmailChanged emailChanged:
                Apply(emailChanged);
                break;
            case StudentEnrolled enrolled:
                Apply(enrolled);
                break;
            case StudentWithdrawn withdrawn:
                Apply(withdrawn);
                break;
        }
    }
    
    private void Apply(StudentCreated @event)
    {
        Id = @event.Id;
        FullName = @event.FullName;
        Email = @event.Email;
        CreatedAtUtc = @event.CreatedAtUtc;
    }
    
    private void Apply(StudentEmailChanged @event)
    {
        Email = @event.Email;
    }

    private void Apply(StudentEnrolled @event)
    {
        if (EnrolledCourses.Contains(@event.CourseName) is false)
            EnrolledCourses.Add(@event.CourseName);
    }
    
    private void Apply(StudentWithdrawn @event)
    {
        if (EnrolledCourses.Contains(@event.CourseName))
            EnrolledCourses.Add(@event.CourseName);
    }
}