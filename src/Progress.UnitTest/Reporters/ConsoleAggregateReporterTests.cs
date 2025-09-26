using Progress.Descriptors;
using Progress.Reporters;
using System.Text;

namespace Progress.UnitTest.Reporters;

public class ConsoleAggregateReporterTests
{
    public ConsoleAggregateReporterTests()
    {
        StringBuilder builder = new();
        TextWriter writer = new StringWriter(builder);
        Console.SetOut(writer);
    }

    [Fact]
    public void GivenDefaults_WhenInitializing_ThenExpectedSettings()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload]);

        // Assert
        reporter.Configuration.Options.DisplayStartingTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayRemainingTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayElapsedTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayEstimatedTimeOfArrival.Should().BeTrue();
        reporter.Configuration.Options.DisplayItemsOverview.Should().BeTrue();
        reporter.Configuration.Options.DisplayItemsSummary.Should().BeTrue();
        reporter.Configuration.Options.HideWorkflowOnComplete.Should().BeFalse();
        reporter.Configuration.Options.NotifyProgressStats.Should().BeFalse();
        reporter.Configuration.Options.NotifyCompletionStats.Should().BeFalse();
        reporter.Configuration.Options.ExportCompletionStats.Should().BeFalse();
        reporter.Configuration.ReportFrequency.Should().Be(TimeSpan.FromSeconds(1));
        reporter.Configuration.StatsFrequency.Should().Be(TimeSpan.FromSeconds(5));
        reporter.Configuration.ExportSettings.Should().BeNull();
        reporter.OnProgress.Should().BeNull();
        reporter.OnCompletion.Should().BeNull();
    }

    [Fact]
    public void GivenNothingToComplete_WhenInitializing_ThenThrowsException()
    {
        // Act
        var workload = Workload.Default(0);
        workload.Component = BarDescriptor.Default.Build();
        var action = () => new ConsoleAggregateReporter([workload]);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNoCollectionOfWorkflows_WhenInitializing_ThenThrowsException()
    {
        // Act
        var action = () => new ConsoleAggregateReporter(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNoWorkflows_WhenInitializing_ThenThrowsException()
    {
        // Act
        var action = () => new ConsoleAggregateReporter([]);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenOperationNotStarted_WhenStopping_ThenThrowsException()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload]);

        // Act
        var action = () => reporter.Stop();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOperationNotStarted_WhenResuming_ThenThrowsException()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload]);

        // Act
        var action = () => reporter.Resume();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOperationNotStopped_WhenResuming_ThenThrowsException()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        using var reporter = new ConsoleAggregateReporter([workload]);
        reporter.Start();

        // Act
        var action = () => reporter.Resume();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenStartedOperation_WhenStopping_ThenSuccess()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload]);
        reporter.Start();

        // Act
        reporter.Stop();
    }

    [Fact]
    public void GivenStoppedOperation_WhenResuming_ThenSuccess()
    {
        // Arrange
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload]);
        reporter.Start();
        reporter.Stop();

        // Act
        reporter.Resume();
    }

    [Fact]
    public async Task GivenProgressNotifications_WhenRunning_ThenCallbackIsCalled()
    {
        // Arrange
        bool isCalled = false;
        var workload = Workload.Default(100);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload])
        {
            OnProgress = (stats) => isCalled = true
        };

        reporter.Configuration.Options.NotifyProgressStats = true;
        reporter.Configuration.StatsFrequency = TimeSpan.FromSeconds(1);
        

        // Act
        reporter.Start();
        await Task.Delay(TimeSpan.FromMilliseconds(1500));

        // Assert
        isCalled.Should().BeTrue();
    }

    [Fact]
    public async Task GivenCompletionNotifications_WhenFinished_ThenCallbackIsCalled()
    {
        // Arrange
        bool isCalled = false;
        var workload = new Workload("Install", "Install stuff", 1);
        workload.Component = BarDescriptor.Default.Build();
        var reporter = new ConsoleAggregateReporter([workload])
        {
            OnCompletion = (stats) => isCalled = true
        };

        reporter.Configuration.Options.NotifyCompletionStats = true;
        reporter.Configuration.ReportFrequency = TimeSpan.FromMicroseconds(500);
        
        reporter.Start();

        // Act
        reporter.ReportSuccess(workload.Id);
        await Task.Delay(TimeSpan.FromMilliseconds(1000));

        // Assert
        isCalled.Should().BeTrue();
    }
}