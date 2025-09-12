using Progress.Builders;
using Progress.Settings;

namespace Progress.UnitTest.Builders;

public class ConsoleReporterBuilderTests
{
    [Fact]
    public void GivenNothingToComplete_WhenBuilding_ThenThrowsException()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder();

        // Act
        var action = () => builder.Build(0);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenSomethingToComplete_WhenBuilding_ThenReturnsReporter()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder();

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public void GivenReportingFrequency_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var expectedFrequency = TimeSpan.FromSeconds(20);
        var builder = new ConsoleReporterBuilder()
            .UsingReportingFrequency(expectedFrequency);

        // Act
        var actual = builder.Build(100).Configuration;

        // Assert
        actual.ReportFrequency.Should().Be(expectedFrequency);
    }

    [Fact]
    public void GivenElapsedTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .DisplayingElapsedTime();

        // Act
        var actual = builder.Build(100).Configuration.Options;

        // Assert
        actual.DisplayElapsedTime.Should().BeTrue();
    }

    [Fact]
    public void GivenStartingTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .DisplayingStartingTime();

        // Act
        var actual = builder.Build(100).Configuration.Options;

        // Assert
        actual.DisplayStartingTime.Should().BeTrue();
    }

    [Fact]
    public void GivenItemsOverview_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .DisplayingItemsOverview();

        // Act
        var actual = builder.Build(100).Configuration.Options;

        // Assert
        actual.DisplayItemsOverview.Should().BeTrue();
    }

    [Fact]
    public void GivenSummary_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleReporterBuilder()
            .DisplayingItemsSummary();

        // Act
        var actual = builder.Build(100).Configuration.Options;

        // Assert
        actual.DisplayItemsSummary.Should().BeTrue();
    }

    [Fact]
    public void GivenProgressNotifications_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var callback = (Stats stats) => { };
        var builder = new ConsoleReporterBuilder()
            .NotifyingProgress(callback);

        // Act
        var actual = builder.Build(100);

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
            .NotifyingProgress(callback, frequency);

        // Act
        var actual = builder.Build(100);

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
            .NotifyingCompletion(callback);

        // Act
        var actual = builder.Build(100);

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
            .ExportingTo(fileName, type);

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.Configuration.ExportSettings.Should().NotBeNull();
        actual.Configuration.ExportSettings.FileName.Should().Be(fileName);
        actual.Configuration.ExportSettings.FileType.Should().Be(FileType.Text);
        actual.Configuration.Options.ExportCompletionStats.Should().BeTrue();
    }
}
