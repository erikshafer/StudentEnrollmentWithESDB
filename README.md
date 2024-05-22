# Student Enrollment Console App with EventStoreDB

An application showing the absolute basics of event sourcing with [EventStoreDB (ESDB)](https://www.eventstore.com/).


## 💡 Inspiration

Inspired by Nick Chapsas' video about [getting started with Event Sourcing in .NET](https://www.youtube.com/watch?v=n_o-xuuVtmw). This take however uses EventStoreDB as its, well, store of events!


### 📄 Mini-Log / 🤔 Thoughts / 🧠 Brain Dump

This is not an Architectural Decision Records (ADRs) log. [This is just a tribute 🎶🎸](https://www.youtube.com/watch?v=Vdq3BtWDRq8).

<details>
  <summary><strong>2024-May-21</strong></summary>
  - For clarity, renamed the .NET console application projects.
</details>
<details>
  <summary><strong>2024-May-21</strong></summary>
  - Updated some code, fixed some bugs and typos.
  - Decided this repository will have multiple versions.
    - One that works nearly the same as Nick's original in-memory incarnation.
    - Another using ESDB and some of the most frequently used commands with the ESDB .NET client.
    - Could have a potential third version if there is some demand from the community.
    - Would appreciate feedback. That and questions could extend this even further, perhaps.
</details>
<details>
  <summary><strong>2024-May-20</strong></summary>
  - Watched Nick's initial video. Awesome. Wait, what if we did the same thing, but with ESDB?
  - Made this code repository and .NET solution.
  - Only the bare bones as of this commit. Will update to include some event sourcing best practices without "getting into the weeds" too much.
</details>


## 📺 Companion Video

A video will be posted on my [YouTube channel](https://www.youtube.com/@event-sourcing) soon. 

Ideally on my birthday, May 22nd!


## 🐋 Requirements

1. [Docker](https://www.docker.com/products/docker-desktop/)
2. [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0+)

## 🏃 Running

### 0️⃣ Project: StudentEnrollment00

Like the other programs that are .NET console application, the database is a glorified key-value store that's in-memory.

To run it, enter the project directory through your shell's associated Change Directory (`cd`) command from the solution root, such as:

```bash
cd .\StudentEnrollment01
```

and then build the project

```bash
dotnet build
```

and run it

```bash
dotnet run
```

Alternatively, you can do all this from the solution's root by targeting the project with `--project` and `run` it immediately, such as:

```bash
dotnet run --project .\StudentEnrollment00\StudentEnrollment00.csproj
```

### 1️⃣ Project: StudentEnrollment01

While still a .NET console app, we're now going to use the Event Store database. So be sure to launch EventStoreDB through Docker. With a terminal / command prompt execute:

```bash
docker-compose up
```

Like before, you can change directory into the project or run it from the root.

Run the console application of your choice, change direction to that particular project directory and then execute:

```bash
cd .\StudentEnrollment01
dotnet build
dotnet run
```

Or alternatively:

```bash
dotnet run --project .\StudentEnrollment01\StudentEnrollment01.csproj
```

### 2️⃣ TBD

Instructions will go here, but will basically be the same as above!


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