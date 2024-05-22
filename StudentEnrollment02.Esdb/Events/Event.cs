namespace StudentEnrollment02.Esdb.Events;

public abstract record Event
{
    public string Id { get; init; } = default!;
    public DateTime CreatedAtUtc { get; set; }
}