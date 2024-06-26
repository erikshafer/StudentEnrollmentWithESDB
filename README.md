# Student Enrollment Console App with EventStoreDB

An application showing the absolute basics of event sourcing with [EventStoreDB (ESDB)](https://www.eventstore.com/).

The accompanying [YouTube video can be found here](https://www.youtube.com/watch?v=SB55-lgK_8I).

## 💡 Inspiration

This effort was inspired by Nick Chapsas' excellent video about [getting started with Event Sourcing in .NET](https://www.youtube.com/watch?v=n_o-xuuVtmw). This version however uses EventStoreDB as its database.

Be sure to check out [Nick's YouTube channel](https://www.youtube.com/@nickchapsas) if you are somehow unfamiliar with his content, as well as his education platform [Dometrain](https://dometrain.com/) where you can level up your software development skills!

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


## 🐋 Requirements

1. [Docker](https://www.docker.com/products/docker-desktop/)
2. [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0+)

## 🏃 Running

### 0️⃣1️⃣ StudentEnrollment01.InMemory

#### AKA the in-memory event store version

Like the other programs that are .NET console application, the database is a glorified key-value store that's in-memory.

To run it, enter the project directory through your shell's associated Change Directory (`cd`) command from the solution root, such as:

```bash
cd .\StudentEnrollment01.InMemory
dotnet build
dotnet run
```

Alternatively, you can do all this from the solution's root by targeting the project with `--project` and `run` it immediately, such as:

```bash
dotnet run --project .\StudentEnrollment01.InMemory\StudentEnrollment01.InMemory.csproj
```

### 0️⃣2️⃣ StudentEnrollment02.Esdb

#### AKA with EventStoreDB

While still a .NET console app, we're now going to use the Event Store database. So be sure to launch EventStoreDB through Docker. With a terminal / command prompt execute:

```bash
docker-compose up
```

**NOTE**: You will need to have ESDB running via Docker for the third (03) project as well.

Like before, you can change directory into the project or run it from the root.

Run the console application of your choice, change direction to that particular project directory and then execute:

```bash
cd .\StudentEnrollment02.Esdb
dotnet build
dotnet run
```

Or alternatively:

```bash
dotnet run --project .\StudentEnrollment02.Esdb\StudentEnrollment02.Esdb.csproj
```

### 0️⃣3️⃣  StudentEnrollment03.Esdb

#### AKA with EventStoreDB, plus a few changes

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