namespace StudentEnrollment04.Esdb.Events;

public abstract record Event
{
    public string Id { get; init; } = default!;
}