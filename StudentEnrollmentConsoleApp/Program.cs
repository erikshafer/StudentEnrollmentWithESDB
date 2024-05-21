using StudentEnrollmentConsoleApp.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var studentId = $"student-{id}";
var now = DateTime.Now;

var studentCreated = new StudentCreated
{
    CreatedAtUtc = now.ToUniversalTime(),
    StudentId = studentId,
    FullName = "Erik Shafer",
    Email = "erik.shafer@eventstore.com",
    DateOfBirth = new DateTime(1987, 1, 1)
};