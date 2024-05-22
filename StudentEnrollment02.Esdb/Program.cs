using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollment02.Esdb.Events;

// Register events to a singleton for ease-of-reference
EventTypeMapper.Instance.ToName(typeof(StudentCreated));
EventTypeMapper.Instance.ToName(typeof(StudentEnrolled));
EventTypeMapper.Instance.ToName(typeof(StudentWithdrawn));
EventTypeMapper.Instance.ToName(typeof(StudentUpdated));

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

// Our EventStoreDB (ESDB)
const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

// Append to ESDB
await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { created, enrolled, emailChanged },
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
Console.WriteLine("Events from selected stream: ");
foreach (var resolved in eventStream)
{
    Console.WriteLine($"\tEventId: {resolved.Event.EventId}");
    Console.WriteLine($"\tEventStreamId: {resolved.Event.EventStreamId}");
    Console.WriteLine($"\tEventType: {resolved.Event.EventType}");
    Console.WriteLine($"\tData: {Encoding.UTF8.GetString(resolved.Event.Data.ToArray())}");
    Console.WriteLine("");
}

// Write out all the courses the student enrolled in
var enrolledCourses = eventStream
    .Where(re => re.Event.EventType == "StudentEnrolled")
    .Select(re => JsonSerializer.Deserialize<StudentEnrolled>(re.Event.Data.ToArray()))
    .Select(se => se!.CourseName)
    .ToList();
Console.WriteLine("Courses enrolled in: ");
enrolledCourses.ForEach(ec => Console.WriteLine($"\t- {ec}"));
Console.WriteLine("");

// Write out using the mapper
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