namespace StudentEnrollmentConsoleApp.Events;

public abstract record Event
{
    public string Id { get; init; } = default!;
}