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
}