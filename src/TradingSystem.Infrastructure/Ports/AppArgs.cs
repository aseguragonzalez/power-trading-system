namespace TradingSystem.Infrastructure.Ports;

public class AppArgs
{
    public const string TimeZoneIdArg = "--timezone";
    public const string TimeZoneIdShortArg = "-t";
    public const string SecondsBetweenReportsArg = "--seconds";
    public const string SecondsBetweenReportsShortArg = "-s";
    public const string RetrySecondsArg = "--retry";
    public const string RetrySecondsShortArg = "-r";
    public const string PathShortArg = "--path";
    public const string PathArg = "-p";

    private readonly List<string> args;

    public string? TimeZoneId { get; }
    public int? SecondsBetweenReports { get; }
    public int? RetrySeconds { get; }
    public string? Path { get; }

    public AppArgs(string[] args)
    {
        this.args = args.ToList() ?? [];

        this.TimeZoneId = GetStringFromArgs([TimeZoneIdArg, TimeZoneIdShortArg]);
        this.SecondsBetweenReports = GetIntFromArgs([SecondsBetweenReportsArg, SecondsBetweenReportsShortArg]);
        this.RetrySeconds = GetIntFromArgs([RetrySecondsArg, RetrySecondsShortArg]);
        this.Path = GetStringFromArgs([PathArg, PathShortArg]);
    }

    private int? GetIntFromArgs(string[] keys)
    {
        string? key = this.args.FirstOrDefault(x => keys.Contains(x));
        if (key is null)
        {
            return null;
        }

        int valueIndex = this.args.IndexOf(key) + 1;
        if (valueIndex >= this.args.Count)
        {
            return null;
        }
        return int.TryParse(this.args[valueIndex], out int value) ? value : null;
    }

    private string? GetStringFromArgs(string[] keys)
    {
        string? key = this.args.FirstOrDefault(x => keys.Contains(x));
        if (key is null)
        {
            return null;
        }

        int valueIndex = this.args.IndexOf(key) + 1;
        if (valueIndex >= this.args.Count)
        {
            return null;
        }
        return this.args[valueIndex];
    }
}
