using Progress.Builders;
using Progress.Settings;

namespace Progress.UnitTest.Builders;

public class ConsoleReporterBuilderTests
{
    [Fact]
    public void GivenNothingToComplete_WhenBuilding_ThenThrowsException()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(0);

        // Act
        var action = () => builder.Build();

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenSomethingToComplete_WhenBuilding_ThenReturnsReporter()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100);

        // Act
        var actual = builder.Build();

        // Assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public void GivenReportingFrequency_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var expectedFrequency = TimeSpan.FromSeconds(20);
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .UsingReportingFrequency(expectedFrequency);

        // Act
        var actual = builder.Build().Configuration;

        // Assert
        actual.ReportFrequency.Should().Be(expectedFrequency);
    }

    [Fact]
    public void GivenElapsedTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .DisplayingElapsedTime();

        // Act
        var actual = builder.Build().Configuration.Options;

        // Assert
        actual.DisplayElapsedTime.Should().BeTrue();
    }

    [Fact]
    public void GivenStartingTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .DisplayingStartingTime();

        // Act
        var actual = builder.Build().Configuration.Options;

        // Assert
        actual.DisplayStartingTime.Should().BeTrue();
    }

    [Fact]
    public void GivenItemsOverview_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .DisplayingItemsOverview();

        // Act
        var actual = builder.Build().Configuration.Options;

        // Assert
        actual.DisplayItemsOverview.Should().BeTrue();
    }

    [Fact]
    public void GivenSummary_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .DisplayingItemsSummary();

        // Act
        var actual = builder.Build().Configuration.Options;

        // Assert
        actual.DisplayItemsSummary.Should().BeTrue();
    }

    [Fact]
    public void GivenProgressNotifications_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var callback = (Stats stats) => { };
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .NotifyingProgress(callback);

        // Act
        var actual = builder.Build();

        // Arrange
        actual.OnProgress.Should().NotBeNull();
        actual.OnProgress.Should().Be(callback);
        actual.Configuration.StatsFrequency.Should().Be(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void GivenProgressNotificationsWithFrequency_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var callback = (Stats stats) => { };
        var frequency = TimeSpan.FromSeconds(20);
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .NotifyingProgress(callback, frequency);

        // Act
        var actual = builder.Build();

        // Arrange
        actual.OnProgress.Should().NotBeNull();
        actual.OnProgress.Should().Be(callback);
        actual.Configuration.StatsFrequency.Should().Be(frequency);
        actual.Configuration.Options.NotifyProgressStats.Should().BeTrue();
    }

    [Fact]
    public void GivenCompletionNotifications_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var callback = (Stats stats) => { };
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .NotifyingCompletion(callback);

        // Act
        var actual = builder.Build();

        // Arrange
        actual.OnCompletion.Should().NotBeNull();
        actual.OnCompletion.Should().Be(callback);
        actual.Configuration.Options.NotifyCompletionStats.Should().BeTrue();
    }

    [Fact]
    public void GivenExporting_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        const string fileName = "output.txt";
        const FileType type = FileType.Text;
        var builder = new ConsoleReporterBuilder()
            .UsingExpectedItems(100)
            .ExportingTo(fileName, type);

        // Act
        var actual = builder.Build();

        // Assert
        actual.Configuration.ExportSettings.Should().NotBeNull();
        actual.Configuration.ExportSettings.FileName.Should().Be(fileName);
        actual.Configuration.ExportSettings.FileType.Should().Be(FileType.Text);
        actual.Configuration.Options.ExportCompletionStats.Should().BeTrue();
    }
}
