using StudentEnrollmentConsoleApp.Events;

namespace StudentEnrollmentConsoleApp;

public sealed class StudentDatabase
{
    private readonly Dictionary<string, SortedList<DateTime, Event>> _studentEvents = new();

    public void Append(Event @event)
    {
        var stream = _studentEvents!.GetValueOrDefault(@event.StreamId, null);
        if (stream is null)
        {
            _studentEvents[@event.StreamId] = new SortedList<DateTime, Event>();
        }

        @event.CreatedAtUtc = DateTime.UtcNow;
        _studentEvents[@event.StreamId].Add(@event.CreatedAtUtc, @event);
    }

    public Student? GetStudent(Guid id)
    {
        var studentId = $"student-{id}";
        
        if (_studentEvents.ContainsKey(studentId) is false)
            return null;

        var student = new Student();
        var studentEvents = _studentEvents[studentId];
        
        foreach (var @event in studentEvents) 
            student.Apply(@event.Value);

        return student;
    }
}