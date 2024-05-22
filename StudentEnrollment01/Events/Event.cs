namespace StudentEnrollment01.Events;

public abstract record Event
{
    public string Id { get; init; } = default!;
}