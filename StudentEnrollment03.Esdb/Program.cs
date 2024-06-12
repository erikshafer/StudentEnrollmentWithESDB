using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollment03.Esdb;
using StudentEnrollment03.Esdb.Events;

EventTypeMapper.Instance.ToName(typeof(StudentCreated));
EventTypeMapper.Instance.ToName(typeof(StudentEnrolled));
EventTypeMapper.Instance.ToName(typeof(StudentWithdrew));
EventTypeMapper.Instance.ToName(typeof(StudentEmailChanged));

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var streamId = $"student-{id}";

var created = new EventData(
    Uuid.NewUuid(),
    nameof(StudentCreated),
    JsonSerializer.SerializeToUtf8Bytes(new StudentCreated
    {
        Id = streamId,
        FullName = "Erik Shafer",
        Email = "erik.shafer@eventstore.com",
        DateOfBirth = new DateTime(1987, 1, 1),
        CreatedAtUtc = DateTime.UtcNow
    })
);

var enrolled = new EventData(
    Uuid.NewUuid(),
    nameof(StudentEnrolled),
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        Id = streamId,
        CourseName = "From Zero to Hero: REST APis in .NET",
        InstructorName = "Nick Chapsas",
        EnrolledAtUtc = DateTime.UtcNow
    })
);

var enrolled2 = new EventData(
    Uuid.NewUuid(),
    nameof(StudentEnrolled),
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        Id = streamId,
        CourseName = "From Zero to Hero: Integration Testing in ASP.NET Core",
        InstructorName = "Nick Chapsas",
        EnrolledAtUtc = DateTime.UtcNow
    })
);

var enrolled3 = new EventData(
    Uuid.NewUuid(),
    nameof(StudentEnrolled),
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        Id = streamId,
        CourseName = "Cloud Fundamentals: AWS Services for C# Developers",
        InstructorName = "Nick Chapsas",
        EnrolledAtUtc = DateTime.UtcNow
    })
);

var emailChanged = new EventData(
    Uuid.NewUuid(),
    "StudentEmailChanged",
    JsonSerializer.SerializeToUtf8Bytes(new StudentEmailChanged
    {
        Id = streamId,
        Email = "erik.shafer.changed.his.email@eventstore.com",
        ChangedAtUtc = DateTime.UtcNow
    })
);

var withdrawn = new EventData(
    Uuid.NewUuid(),
    nameof(StudentWithdrew),
    JsonSerializer.SerializeToUtf8Bytes(new StudentWithdrew
    {
        Id = streamId,
        CourseName = "Cloud Fundamentals: AWS Services for C# Developers",
        WithdrawnAtUtc = DateTime.UtcNow
    })
);

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

// Append the initial batch of events.
await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { created, enrolled, enrolled2, enrolled3, emailChanged, withdrawn },
    cancellationToken: default
);

// Read from the stream we just appended to.
var streamResult = client.ReadStreamAsync(
    Direction.Forwards,
    streamId,
    StreamPosition.Start,
    cancellationToken: default
);

// Optional safety check, but here we're ensuring the stream was found.
if (await streamResult.ReadState is ReadState.StreamNotFound)
{
    Console.WriteLine($"The fetched stream (id: {streamId}) that was read was in StreamNotFound state");
    return;
}

// Okay, taking that StreamResult we're going to make it a List of ResolvedEvents.
var eventStream = await streamResult.ToListAsync();
Console.WriteLine($"Events (total: {eventStream.Count}) from selected stream: ");

// Instantiate our model and then apply state changes from the deserialized events.
var student = new Student();
foreach (var @event in eventStream)
{
    var deserializeEvent = DeserializeEvent(@event.Event);
    switch (deserializeEvent)
    {
        case StudentCreated studentCreated:
            student.Apply(studentCreated);
            break;
        case StudentEnrolled studentEnrolled:
            student.Apply(studentEnrolled);
            break;
        case StudentEmailChanged studentEmailChanged:
            student.Apply(studentEmailChanged);
            break;
        case StudentWithdrew studentWithdrew:
            student.Apply(studentWithdrew);
            break;
    }
}

Console.WriteLine(
    "StudentId: {0}\nFullName: {1}\nEmail: {2}\nDateOfBirth: {3}\nCreatedAtUtc: {4}", 
    student!.Id, student.FullName, student.Email, student.DateOfBirth, student.CreatedAtUtc);
Console.WriteLine("Enrolled courses:");
foreach (var enrolledCourse in student.EnrolledCourses) 
    Console.WriteLine($"\t- {enrolledCourse}");
Console.WriteLine();

static Event DeserializeEvent(EventRecord eventRecord)
{
    return (Event)JsonSerializer
        .Deserialize(
            Encoding.UTF8.GetString(eventRecord.Data.ToArray()),
            Type.GetType($"{typeof(Event).Namespace}.{eventRecord.EventType}")!)!;
}