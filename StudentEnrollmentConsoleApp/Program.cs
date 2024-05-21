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

var inMemoryDb = new StudentDatabase();
inMemoryDb.Append(studentCreated);

const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);