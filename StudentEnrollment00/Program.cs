using StudentEnrollment00;
using StudentEnrollment00.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var now = DateTime.Now;

var studentCreated = new StudentCreated
{
    CreatedAtUtc = now.ToUniversalTime(),
    StudentId = id,
    FullName = "Erik Shafer",
    Email = "erik.shafer@eventstore.com",
    DateOfBirth = new DateTime(1987, 1, 1)
};

var inMemoryDb = new InMemoryDatabase();
inMemoryDb.Append(studentCreated);

var student = inMemoryDb.GetStudent(id);

Console.WriteLine(
    "StudentId: {0} | FullName: {1} | Email: {2} | DOB: {3} | CreatedAtUtc: {4}", 
    student!.Id, student.FullName, student.Email, student.DateOfBirth, student.CreatedAtUtc);
Console.WriteLine();