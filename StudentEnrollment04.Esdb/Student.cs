using StudentEnrollment04.Esdb.Events;

namespace StudentEnrollment04.Esdb;

public class Student
{
    internal Student()
    {
    }
    
    public string Id { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public DateTime DateOfBirth { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IList<CourseCode> EnrolledCourses { get; private set; } = [];

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
        var enrollingCourseCode = new CourseCode(@event.CourseName);

        var existingCourseCode = FindCourseCodeMatchingWith(enrollingCourseCode);

        if (existingCourseCode is null)
        {
            EnrolledCourses.Add(enrollingCourseCode);
            return;
        }
        
        var indexOfExistingCode = EnrolledCourses.IndexOf(existingCourseCode);

        if (indexOfExistingCode == -1)
            throw new ArgumentOutOfRangeException(nameof(existingCourseCode), "Course Code was not found");

        EnrolledCourses[indexOfExistingCode] = enrollingCourseCode;
    }
    
    private void Apply(StudentWithdrawn @event)
    {
        var withdrawingCourseCode = new CourseCode(@event.CourseName);

        var existingCourseCode = FindCourseCodeMatchingWith(withdrawingCourseCode);
        
        if (existingCourseCode is null)
        {
            throw new InvalidOperationException($"Course Code of `{withdrawingCourseCode.Value}` was not found in student's enrolled courses");
        }
    }

    private CourseCode? FindCourseCodeMatchingWith(CourseCode courseCode)
        => EnrolledCourses.SingleOrDefault(ec => ec.HasSameValue(courseCode));
}