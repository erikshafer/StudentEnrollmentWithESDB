# Student Enrollment Console App with EventStoreDB

An application showing the absolute basics of event sourcing with [EventStoreDB (ESDB)](https://www.eventstore.com/).

## 💡 Inspiration

Inspired by Nick Chapsas' video about [getting started with Event Sourcing in .NET](https://www.youtube.com/watch?v=n_o-xuuVtmw). This take however uses EventStoreDB as its, well, store of events!

### 🤔 Mini-Log / Thoughts / Brian Dump

- **2024-May-21**:
  - Updated some code, fixed some bugs and typos.
  - Decided this repository will have multiple versions.
    - One that works nearly the same as Nick's original in-memory incarnation.
    - Another using ESDB and some of the most frequently used commands with the ESDB .NET client.
    - Could have a potential third version if there is some demand from the community.
    - Would appreciate feedback. That and questions could extend this even further, perhaps.
- **2024-May-20**:
  - Watched Nick's initial video. Awesome. Wait, what if we did the same thing, but with ESDB?
  - Made this code repository and .NET solution.
  - Only the bare bones as of this commit. Will update to include some event sourcing best practices without "getting into the weeds" too much.

## 📺 Companion Video

A video will be posted on my [YouTube channel](https://www.youtube.com/@event-sourcing) soon. 

Ideally on my birthday, May 22nd!

## 🐋 Requirements

1. [Docker](https://www.docker.com/products/docker-desktop/)
2. [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0+)

## 🏃 Running

Be sure to launch EventStoreDB through Docker. Through a terminal / command prompt:

```bash
docker-compose up
```

You can build the entire solution, including all versions of the projects, by running the following at the root of `/StudentEnrollmentConsoleApp/` with:

```bash
dotnet build
```

Run the console application of your choice, change direction to that particular project directory and then execute:

```bash
dotnet run
```

Alternatively, you can stay in the root directory `/StudentEnrollmentConsoleApp/` and include the path to the intended project you want to have run. This same operation can be done with the previous `dotnet build` command, too.

```bash
dotnet run .\StudentEnrollment01\StudentEnrollment01.csproj
```

## 🔗 Resources

Erik Shafer (me):

- [event-sourcing.dev](https://event-sourcing.dev) - Blog, presentations, contact information
- [youtube.com/@event-sourcing](https://youtube.com/@event-sourcing) - YouTube channel

Nick Chapsas:

- [youtube.com/@nickchapsas](https://www.youtube.com/@nickchapsas) - YouTube channel
- [Dometrain](https://dometrain.com/) - Courses crafted by real engineers for the real world.

## License

To Be Determined.

This is done in the open to help educate. Don't be a jerk. Etc. :)