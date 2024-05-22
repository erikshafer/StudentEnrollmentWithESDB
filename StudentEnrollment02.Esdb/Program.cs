using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollment02.Esdb.Events;

// Validate GUID shape, then stringify. Same exact value as the in-memory version (01).
var streamId = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908").ToString();

// Create the student events,
// serialize them to JSON,
// and then place them inside the EventData object from the ESDB client.
var created = new EventData(
    Uuid.NewUuid(),
    "StudentCreated",
    JsonSerializer.SerializeToUtf8Bytes(new StudentCreated
    {
        StudentId = streamId,
        FullName = "Erik Shafer",
        Email = "erik.shafer@eventstore.com",
        DateOfBirth = new DateTime(1987, 1, 1),
        CreatedAtUtc = DateTime.UtcNow
    })
);

var enrolled = new EventData(
    Uuid.NewUuid(),
    "StudentEnrolled",
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        StudentId = streamId,
        CourseName = "From Zero to Hero: REST APis in .NET",
        CreatedAtUtc = DateTime.UtcNow
    })
);

var emailChanged = new EventData(
    Uuid.NewUuid(),
    "StudentEmailChanged",
    JsonSerializer.SerializeToUtf8Bytes(new StudentUpdated
    {
        StudentId = streamId,
        FullName = "Erik Shafer",
        Email = "erik.shafer.changed.his.email@eventstore.com",
        CreatedAtUtc = DateTime.UtcNow
    })
);

// Our EventStoreDB (ESDB) connection details.
const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

// Append the events to ESDB.
// The StreamState is set to Any, but it could also be
// NoStream to ensure nothing exists (otherwise an exception will throw), or
// StreamExists to ensure it isn't new (AKA being created right through this action).
await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { created, enrolled, emailChanged },
    cancellationToken: default
);

// Read from ESDB. Specifically from the start of the stream
// we've been working with for our example.
var readStreamResult = client.ReadStreamAsync(
    Direction.Forwards,
    streamId,
    StreamPosition.Start,
    cancellationToken: default
);
var eventStream = await readStreamResult.ToListAsync();

// Write out the events from the stream.
// We're printing out to Console exactly as-is from the ESDB client.
// No mapping to a C# object, like Student, or anything like that. Yet.
Console.WriteLine("Events from selected stream: ");
foreach (var resolved in eventStream)
{
    Console.WriteLine($"\tEventId: {resolved.Event.EventId}");
    Console.WriteLine($"\tEventStreamId: {resolved.Event.EventStreamId}");
    Console.WriteLine($"\tEventType: {resolved.Event.EventType}");
    Console.WriteLine($"\tData: {Encoding.UTF8.GetString(resolved.Event.Data.ToArray())}");
    Console.WriteLine("");
}

// An example of searching through the events for a specific 
// event type and then for a specific value in that type of event.
// AKA, writing out all the courses our student enrolled in.
var enrolledCourses = eventStream
    .Where(re => re.Event.EventType == "StudentEnrolled")
    .Select(re => JsonSerializer.Deserialize<StudentEnrolled>(re.Event.Data.ToArray()))
    .Select(se => se!.CourseName)
    .ToList();
Console.WriteLine("Courses enrolled in: ");
enrolledCourses.ForEach(ec => Console.WriteLine($"\t- {ec}"));
Console.WriteLine("");