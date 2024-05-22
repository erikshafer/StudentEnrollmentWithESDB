using StudentEnrollment01.InMemory.Events;

namespace StudentEnrollment01.InMemory;

public sealed class InMemoryDatabase
{
    private readonly Dictionary<Guid, SortedList<DateTime, Event>> _studentEvents = new();

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
        if (_studentEvents.ContainsKey(id) is false)
            return null;

        var student = new Student();
        var studentEvents = _studentEvents[id];

        foreach (var @event in studentEvents) 
            student.Apply(@event.Value);

        return student;
    }
}