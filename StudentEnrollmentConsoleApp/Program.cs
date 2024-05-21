using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollmentConsoleApp;
using StudentEnrollmentConsoleApp.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var streamId = $"student-{id}";

var created = new StudentCreated
{
    StudentId = streamId,
    FullName = "Erik Shafer",
    Email = "erik.shafer@eventstore.com",
    DateOfBirth = new DateTime(1987, 1, 1)
};
var createdEventData = new EventData(
    Uuid.NewUuid(),
    "StudentCreated",
    JsonSerializer.SerializeToUtf8Bytes(created)
);

var enrolled = new StudentEnrolled
{
    StudentId = streamId,
    CourseName = "From Zero to Hero: REST APis in .NET"
};
var enrolledEventData = new EventData(
    Uuid.NewUuid(),
    "StudentEnrolled",
    JsonSerializer.SerializeToUtf8Bytes(enrolled)
);

var updated = new StudentUpdated
{
    StudentId = streamId,
    FullName = "Erik Shafer",
    Email = "erik.shafer.new.email@eventstore.com",
};
var updatedEventData = new EventData(
    Uuid.NewUuid(),
    "StudentUpdated", 
    JsonSerializer.SerializeToUtf8Bytes(updated)
);

// our in-memory database
var inMemoryDb = new StudentDatabase();
inMemoryDb.Append(created);
inMemoryDb.Append(enrolled);
inMemoryDb.Append(updated);

// our EventStoreDB (ESDB)
const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

// append to ESDB
await client.AppendToStreamAsync(
    streamId,
    StreamState.Any,
    new[] { createdEventData, enrolledEventData, updatedEventData },
    cancellationToken: default
);

// read from ESDB
var result = client.ReadStreamAsync(
    Direction.Forwards,
    streamId,
    StreamPosition.Start,
    cancellationToken: default
);

var events = await result.ToListAsync();

foreach (var @event in events)
{
    Console.WriteLine($"EventId: {@event.Event.EventId}");
    Console.WriteLine($"EventStreamId: {@event.Event.EventStreamId}");
    Console.WriteLine($"EventType: {@event.Event.EventType}");
    // var data = JsonSerializer.Deserialize<StudentCreated>(@event.Event.Data.ToArray()); // TODO
    var data = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
    Console.WriteLine($"Data: {data}");
    Console.WriteLine("---");
}

var studentFromInMemDb = inMemoryDb.GetStudent(id);
Console.Write(studentFromInMemDb);
Console.WriteLine("");

var studentFromEsdb = new Student();
foreach (var @event in events)
{
    var eventType = @event.Event.EventType;
    var dataUtf8 = @event.Event.Data.ToArray();
    switch (eventType)
    {
        case "studentCreated":
        {
            var deserialized = JsonSerializer.Deserialize<StudentCreated>(dataUtf8);
            studentFromEsdb.Apply(deserialized!);
            break;
        }
        case "studentEnrolled":
        {
            var deserialized = JsonSerializer.Deserialize<StudentEnrolled>(dataUtf8);
            studentFromEsdb.Apply(deserialized!);
            break;
        }
        case "studentUpdated":
        {
            var deserialized = JsonSerializer.Deserialize<StudentUpdated>(dataUtf8);
            studentFromEsdb.Apply(deserialized!);
            break;
        }
        case "studentUnEnrolled":
        {
            var deserialized = JsonSerializer.Deserialize<StudentUnEnrolled>(dataUtf8);
            studentFromEsdb.Apply(deserialized!);
            break;
        }
    }
}

Console.Write(studentFromEsdb);
Console.WriteLine("");