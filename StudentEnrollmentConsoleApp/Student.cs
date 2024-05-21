using StudentEnrollmentConsoleApp.Events;

namespace StudentEnrollmentConsoleApp;

public class Student
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<string> EnrolledCourses { get; set; } = new();

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