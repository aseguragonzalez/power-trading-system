# Power Trading system

## Pre-requisites

- [Docker][docker]
- [Dev Container][devcontainer]
- [Visual Studio Code][vscode]

## Getting Started

This project was built using DevContainer in Visual Studio Code. To start developing, you need to open the project in Visual Studio Code and click on the "Reopen in Container" button.

To get started with development, you can use the following commands:

### Restore the dependencies

```bash
dotnet restore
```

### Building the project

```bash
dotnet build
```

### Running tests

```bash
dotnet test
```

## Running the Application

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

Sample command:

```bash
./TradingSystem.App -p /tmp/reports -t Europe/Madrid -s 10 -r 5
```

## Build With

- [Dotnet Core][dotnet]
- [xUnit][xunit]

[devcontainer]: https://code.visualstudio.com/docs/remote/containers
[docker]: https://www.docker.com/
[dotnet]: https://dotnet.microsoft.com/download
[timezones]: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
[vscode]: https://code.visualstudio.com/
[xunit]: https://xunit.net/
