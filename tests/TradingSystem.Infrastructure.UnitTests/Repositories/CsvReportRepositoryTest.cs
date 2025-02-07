using System.Globalization;
using FluentAssertions;
using TradingSystem.Domain;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Repositories;

namespace TradingSystem.Infrastructure.UnitTests.Repositories;

public class CsvReportRepositoryTest
{
    [Fact]
    public void ShouldFailsWhenSettingsAreMissing()
    {
        // Arrange
        Action act = () => _ = new CsvReportRepository(null!);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        CsvReportRepositorySettings settings = new(directory: "reports");

        // Act
        CsvReportRepository repository = new(settings);

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldFailWhenSaveWithoutReport()
    {
        // Arrange
        CsvReportRepositorySettings settings = new(directory: "reports");
        CsvReportRepository repository = new(settings);
        Report report = new(date: DateTime.UtcNow.AddDays(1), offset: 0);
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
        CsvReportRepository repository = new(settings);
        Report report = new(date: DateTime.UtcNow.AddDays(1), offset: 0);
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
