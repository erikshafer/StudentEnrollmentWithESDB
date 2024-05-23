using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollment02.Esdb.Events;

var streamId = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908").ToString();

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

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { created, enrolled, emailChanged },
    cancellationToken: default
);

var streamResult = client.ReadStreamAsync(
    Direction.Forwards,
    streamId,
    StreamPosition.Start,
    cancellationToken: default
);

if (await streamResult.ReadState is ReadState.StreamNotFound)
{
    Console.WriteLine($"The fetched stream (id: {streamId}) that was read was in StreamNotFound state");
    return;
}

var eventStream = await streamResult.ToListAsync();

Console.WriteLine($"Events (total: {eventStream.Count}) from selected stream: ");
foreach (var resolved in eventStream)
{
    Console.WriteLine($"\tEventId: {resolved.Event.EventId}");
    Console.WriteLine($"\tEventStreamId: {resolved.Event.EventStreamId}");
    Console.WriteLine($"\tEventType: {resolved.Event.EventType}");
    Console.WriteLine($"\tData: {Encoding.UTF8.GetString(resolved.Event.Data.ToArray())}");
    Console.WriteLine("");
}

var enrolledCourses = eventStream
    .Where(re => re.Event.EventType == "StudentEnrolled")
    .Select(re => JsonSerializer.Deserialize<StudentEnrolled>(re.Event.Data.ToArray()))
    .Select(se => se!.CourseName)
    .ToList();
Console.WriteLine("Enrolled courses found in the stream: ");
enrolledCourses.ForEach(ec => Console.WriteLine($"\t- {ec}")); // run the app multiple times ;)
Console.WriteLine("");