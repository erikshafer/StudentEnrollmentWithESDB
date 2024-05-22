using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollment03.Esdb.Events;

// Register events to a singleton for ease-of-reference
EventTypeMapper.Instance.ToName(typeof(StudentCreated));
EventTypeMapper.Instance.ToName(typeof(StudentEnrolled));
EventTypeMapper.Instance.ToName(typeof(StudentWithdrawn));
EventTypeMapper.Instance.ToName(typeof(StudentEmailChanged));

// Similar to before, we have the same exact GUID/UUID, but
// now we're prepending `student-` in front.
// Why? We don't have your typical relational tables in ESDB,
// so we're effectively putting the entity (table) name in front.
// You can do all sorts of things by having a well crafted
// naming structure for streams, including versions, dates, or
// whatever else may provide clarity to those in your domain.
var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var streamId = $"student-{id}";

// Create the student events,
// serialize them to JSON,
// and then place them inside the EventData object from the ESDB client.
var created = new EventData(
    Uuid.NewUuid(),
    "StudentCreated",
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
    "StudentEnrolled",
    JsonSerializer.SerializeToUtf8Bytes(new StudentEnrolled
    {
        Id = streamId,
        CourseName = "From Zero to Hero: REST APis in .NET",
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

// Write out using the mapper.
Console.WriteLine("Deserialized events:");
foreach (var resolved in eventStream)
{
    var eventType = EventTypeMapper.Instance.ToType(resolved.Event.EventType);

    if (eventType == null)
        break;

    var deserializedEvent = JsonSerializer.Deserialize(Encoding.UTF8.GetString(resolved.Event.Data.Span), eventType);
    
    Console.WriteLine($"\t{deserializedEvent}");
}

Console.WriteLine("");