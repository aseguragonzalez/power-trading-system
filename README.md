# Power Trading system

## Pre-requisites

- [Docker][docker]
- [Dev Container][devcontainer]
- [Pre-commit][pre-commit]
- [Visual Studio Code][vscode]

## Getting Started

This project was built using DevContainer in Visual Studio Code. To start developing, you need to open the project in Visual Studio Code and click on the "Reopen in Container" button.

To get started with development, you can use the following commands:

### Restore dependencies

```bash
dotnet restore
```

### Building project

```bash
dotnet build
```

### Running tests

```bash
dotnet test
```

## Running the Application

### Using Docker

To build the Docker image, you can use the following command:

```bash
docker build -t trading-system:latest .
```

To run the app in a container, you can use the following command:

```bash
docker run --env-file .env -v ./reports:/app/reports trading-system:latest
```

### Environment variables

You have to define the following environment variables with the default values the app will use:

- `CSV_REPORTS_DIRECTORY`: The directory where the reports will be stored.
- `SECONDS_BETWEEN_REPORTS`: The seconds between reports.
- `SECONDS_BETWEEN_RETRIES`: The seconds between retries when the app fails getting the power positions.
- `TIME_ZONE`: The timezone to use when generating the reports. Use the [IANA time zone database][timezones] to set the value.

### Optional arguments

Also, you can use the following optional arguments when running the app:

- Reports path: `-p <path>` or `--path <path>`
- Timezone: `-t <timezone>` or `--timezone <timezone>`
- Seconds between reports: `-s <seconds>` or `--seconds <seconds>`
- Seconds between retries: `-r <seconds>` or `--retries <seconds>`

Example command:

```bash
./TradingSystem.App -p /tmp/reports -t Europe/Madrid -s 10 -r 5
```

## Build With

- [AutoFixture][auto-fixture]
- [Dotnet Core][dotnet]
- [Fluent Assertions][fluent-assertions]
- [NSubsitute][nsubstitute]
- [Pre-commit][pre-commit]
- [xUnit][xunit]

[auto-fixture]: https://github.com/AutoFixture/AutoFixture
[devcontainer]: https://code.visualstudio.com/docs/remote/containers
[docker]: https://www.docker.com/
[dotnet]: https://dotnet.microsoft.com/download
[fluent-assertions]: https://github.com/fluentassertions/fluentassertions
[nsubstitute]: https://nsubstitute.github.io/
[pre-commit]: https://pre-commit.com/
[timezones]: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
[vscode]: https://code.visualstudio.com/
[xunit]: https://xunit.net/
