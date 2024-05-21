using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollmentConsoleApp.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var streamId = $"student-{id}";

var created = new EventData(
    Uuid.NewUuid(),
    "StudentCreated",
    JsonSerializer.SerializeToUtf8Bytes(new StudentCreated
    {
        StudentId = streamId,
        FullName = "Erik Shafer",
        Email = "erik.shafer@eventstore.com",
        DateOfBirth = new DateTime(1987, 1, 1)
    })
);

var enrolled = new EventData(
    Uuid.NewUuid(),
    "StudentEnrolled",
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        StudentId = streamId,
        CourseName = "From Zero to Hero: REST APis in .NET"
    })
);

var updated = new EventData(
    Uuid.NewUuid(),
    "StudentUpdated", 
    JsonSerializer.SerializeToUtf8Bytes(new StudentUpdated
    {
        StudentId = streamId,
        FullName = "Erik Shafer",
        Email = "erik.shafer.new.email@eventstore.com",
    })
);

// Our EventStoreDB (ESDB)
const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

// Append to ESDB
await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { created, enrolled, updated },
    cancellationToken: default
);

// Read from ESDB
var readStreamResult = client.ReadStreamAsync(
    Direction.Forwards,
    streamId,
    StreamPosition.Start,
    cancellationToken: default
);
var eventStream = await readStreamResult.ToListAsync();

// Write out the events from the stream
foreach (var @event in eventStream)
{
    Console.WriteLine($"EventId: {@event.Event.EventId}");
    Console.WriteLine($"EventStreamId: {@event.Event.EventStreamId}");
    Console.WriteLine($"EventType: {@event.Event.EventType}");
    Console.WriteLine($"Data: {Encoding.UTF8.GetString(@event.Event.Data.ToArray())}");
    Console.WriteLine("---");
}

// Write out all the courses the student enrolled in
var enrolledCourses = eventStream
    .Where(re => re.Event.EventType == "StudentEnrolled")
    .Select(re => JsonSerializer.Deserialize<StudentEnrolled>(re.Event.Data.ToArray()))
    .Select(se => se!.CourseName)
    .ToArray();

Console.WriteLine($"Courses enrolled in: {enrolledCourses}");
Console.WriteLine("");