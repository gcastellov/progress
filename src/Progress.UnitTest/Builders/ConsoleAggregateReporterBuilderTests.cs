using Progress.Builders;
using Progress.Descriptors;
using Progress.Reporters;
using Progress.Settings;

namespace Progress.UnitTest.Builders;

public class ConsoleAggregateReporterBuilderTests
{
    [Fact]
    public void GivenNothingToComplete_WhenBuilding_ThenThrowsException()
    {
        // Arrange
        var builder = new ConsoleAggregateReporterBuilder();

        // Act
        var action = () => builder.Build();

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenSomethingToComplete_WhenBuilding_ThenReturnsReporter()
    {
        // Arrange
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default);

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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
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
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
            .ExportingTo(fileName, type);

        // Act
        var actual = builder.Build();

        // Assert
        actual.Configuration.ExportSettings.Should().NotBeNull();
        actual.Configuration.ExportSettings.FileName.Should().Be(fileName);
        actual.Configuration.ExportSettings.FileType.Should().Be(FileType.Text);
        actual.Configuration.Options.ExportCompletionStats.Should().BeTrue();
    }

    [Fact]
    public void GivenHideOnComplete_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ConsoleAggregateReporterBuilder()
            .UsingWorkload(Workload.Default(100), BarDescriptor.Default)
            .HideWorkloadOnComplete(true);

        // Act
        var actual = builder.Build().Configuration.Options;

        // Assert
        actual.HideWorkflowOnComplete.Should().BeTrue();
    }
}
