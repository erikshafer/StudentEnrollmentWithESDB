using System.Text;
using System.Text.Json;
using EventStore.Client;
using StudentEnrollmentConsoleApp;
using StudentEnrollmentConsoleApp.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var studentId = $"student-{id}";
var now = DateTime.Now;

var studentCreated = new StudentCreated
{
    StudentId = studentId,
    FullName = "Erik Shafer",
    Email = "erik.shafer@eventstore.com",
    DateOfBirth = new DateTime(1987, 1, 1)
};
const string eventType = nameof(studentCreated); //"StudentCreated";
const string streamName = "some-stream";

var inMemoryDb = new StudentDatabase();
inMemoryDb.Append(studentCreated);

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

var eventData = new EventData(
    Uuid.NewUuid(),
    eventType,
    JsonSerializer.SerializeToUtf8Bytes(studentCreated)
);

await client.AppendToStreamAsync(
    streamName,
    StreamState.Any,
    new[] { eventData },
    cancellationToken: default
);

var result = client.ReadStreamAsync(
    Direction.Forwards,
    streamName,
    StreamPosition.Start,
    cancellationToken: default
);

var events = await result.ToListAsync();

foreach (var @event in events)
{
    Console.WriteLine($"EventType: {@event.Event.EventId}");
    Console.WriteLine($"EventStreamId: {@event.Event.EventStreamId}");
    Console.WriteLine($"EventType: {@event.Event.EventType}");
    var data = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
    Console.WriteLine($"Data: {data}");
    Console.WriteLine("-----");
}
