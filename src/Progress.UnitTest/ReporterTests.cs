using Progress.Descriptors;
using System.Text;

namespace Progress.UnitTest;

public class ReporterTests
{
    public ReporterTests()
    {
        StringBuilder builder = new();
        TextWriter writer = new StringWriter(builder);
        Console.SetOut(writer);
    }

    [Fact]
    public void GivenDefaults_WhenInitializing_ThenExpectedSettings()
    {
        // Arrange
        var reporter = new Reporter(100, BarDescriptor.Default.Build());

        // Assert        
        reporter.Configuration.Options.DisplayStartingTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayRemainingTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayElapsedTime.Should().BeTrue();
        reporter.Configuration.Options.DisplayEstimatedTimeOfArrival.Should().BeTrue();
        reporter.Configuration.Options.DisplayItemsOverview.Should().BeTrue();
        reporter.Configuration.Options.DisplayItemsSummary.Should().BeTrue();
        reporter.Configuration.Options.NotifyProgressStats.Should().BeTrue();
        reporter.Configuration.Options.NotifyCompletionStats.Should().BeTrue();
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
        var action = () => new Reporter(0, BarDescriptor.Default.Build());

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNoComponent_WhenInitializing_ThenThrowsException()
    {
        // Act
        var action = () => new Reporter(100, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenOperationNotStarted_WhenStopping_ThenThrowsException()
    {
        // Arrange
        var reporter = new Reporter(100, BarDescriptor.Default.Build());

        // Act
        var action = () => reporter.Stop();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOperationNotStarted_WhenResuming_ThenThrowsException()
    {
        // Arrange
        var reporter = new Reporter(100, BarDescriptor.Default.Build());

        // Act
        var action = () => reporter.Resume();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOperationNotStopped_WhenResuming_ThenThrowsException()
    {
        // Arrange
        using var reporter = new Reporter(100, BarDescriptor.Default.Build());
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
        var reporter = new Reporter(100, BarDescriptor.Default.Build());
        reporter.Start();

        // Act
        reporter.Stop();
    }

    [Fact]
    public void GivenStoppedOperation_WhenResuming_ThenSuccess()
    {
        // Arrange
        var reporter = new Reporter(100, BarDescriptor.Default.Build());
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
        var reporter = new Reporter(100, BarDescriptor.Default.Build())
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
        var reporter = new Reporter(1, BarDescriptor.Default.Build())
        {
            OnCompletion = (stats) => isCalled = true
        };

        reporter.Configuration.Options.NotifyCompletionStats = true;
        reporter.Configuration.ReportFrequency = TimeSpan.FromMicroseconds(500);
        
        reporter.Start();

        // Act
        reporter.ReportSuccess();
        await Task.Delay(TimeSpan.FromMilliseconds(1000));

        // Assert
        isCalled.Should().BeTrue();
    }
}