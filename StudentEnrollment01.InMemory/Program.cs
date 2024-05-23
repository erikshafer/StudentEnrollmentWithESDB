using StudentEnrollment01.InMemory;
using StudentEnrollment01.InMemory.Events;

var id = Guid.Parse("a662d446-4920-415e-8c2a-0dd4a6c58908");
var now = DateTime.Now.ToUniversalTime();

var studentCreated = new StudentCreated
{
    CreatedAtUtc = now,
    StudentId = id,
    FullName = "Erik Shafer",
    Email = "erik.shafer@eventstore.com",
    DateOfBirth = new DateTime(1987, 1, 1)
};

var inMemoryDb = new InMemoryDatabase();
inMemoryDb.Append(studentCreated);

var student = inMemoryDb.GetStudent(id);

Console.WriteLine(
    "StudentId: {0}\nFullName: {1}\nEmail: {2}\nDateOfBirth: {3}\nCreatedAtUtc: {4}", 
    student!.Id, student.FullName, student.Email, student.DateOfBirth, student.CreatedAtUtc);
Console.WriteLine("Enrolled courses:");
if (student.EnrolledCourses.Count is 0)
{
    Console.WriteLine("No enrolled courses");
    return;
}

foreach (var enrolledCourse in student.EnrolledCourses) 
    Console.WriteLine($"\t- {enrolledCourse}");
Console.WriteLine();