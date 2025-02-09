using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TradingSystem.Domain.Entities;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Adapters.Repositories;

namespace TradingSystem.UnitTests.Infrastructure.Adapters.Repositories;

public class CsvReportRepositoryTest
{
    [Fact]
    public void ShouldFailsWhenSettingsAreMissing()
    {
        // Arrange
        ILogger<CsvReportRepository> logger = Substitute.For<ILogger<CsvReportRepository>>();
        Action act = () => _ = new CsvReportRepository(null!, logger);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        ILogger<CsvReportRepository> logger = Substitute.For<ILogger<CsvReportRepository>>();
        CsvReportRepositorySettings settings = new(directory: "reports");

        // Act
        CsvReportRepository repository = new(settings, logger);

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldFailWhenSaveWithoutReport()
    {
        // Arrange
        ILogger<CsvReportRepository> logger = Substitute.For<ILogger<CsvReportRepository>>();
        CsvReportRepositorySettings settings = new(directory: "reports");
        CsvReportRepository repository = new(settings, logger);
        Func<Task> act = async () => await repository.Save(null!);

        // Act & Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'report')");
    }

    [Fact]
    public async Task ShouldCreateAFileReport()
    {
        // Arrange
        string directoryPath = $"./reports_{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)}";
        CsvReportRepositorySettings settings = new(directory: directoryPath);
        ILogger<CsvReportRepository> logger = Substitute.For<ILogger<CsvReportRepository>>();
        CsvReportRepository repository = new(settings, logger);
        Report report = new(date: DateTime.UtcNow.AddDays(1), offset: TimeSpan.Zero);
        report.AddTradePositions(new TradePositions(
            [
                new TradePosition(1, 100),
                new TradePosition(2, 100),
                new TradePosition(3, 100),
                new TradePosition(4, 100),
                new TradePosition(5, 100),
                new TradePosition(6, 100),
                new TradePosition(7, 100),
                new TradePosition(8, 100),
                new TradePosition(9, 100),
                new TradePosition(10, 100),
                new TradePosition(11, 100),
                new TradePosition(12, 100),
            ]
        ));

        // Act
        await repository.Save(report);

        // Assert
        string filePath = Path.Combine(settings.Directory, report.ReportName);
        File.Exists(filePath).Should().BeTrue();
        using FileStream csvReportStream = File.OpenRead(filePath);
        using StreamReader reader = new(csvReportStream);
        string? header = await reader.ReadLineAsync();
        header.Should().NotBeNull();
        header.Should().Be("Datetime;Volume");
        IEnumerable<string> lines = (await reader.ReadToEndAsync()).Split(Environment.NewLine).Where(line => !string.IsNullOrEmpty(line));
        lines.Count().Should().Be(report.GetPositions().Count());
    }
}
