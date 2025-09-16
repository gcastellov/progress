using Progress.Descriptors;
using Progress.Reporters;

namespace Progress.UnitTest.Reporters;

public class BackgroundReporterTests
{
    [Fact]
    public void GivenNothingToComplete_WhenInitializing_ThenThrowsException()
    {
        // Act
        var action = () => new BackgroundReporter(0);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenOperationNotStarted_WhenStopping_ThenThrowsException()
    {
        // Arrange
        var reporter = new BackgroundReporter(100);

        // Act
        var action = () => reporter.Stop();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task GivenProgressNotifications_WhenRunning_ThenCallbackIsCalled()
    {
        // Arrange
        bool isCalled = false;
        var reporter = new BackgroundReporter(100)
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
        var reporter = new BackgroundReporter(1)
        {
            OnCompletion = (stats) => isCalled = true
        };

        reporter.Configuration.Options.NotifyCompletionStats = true;
        reporter.Configuration.StatsFrequency = TimeSpan.FromMilliseconds(500);
        reporter.Start();

        // Act
        reporter.ReportSuccess();
        await Task.Delay(TimeSpan.FromMilliseconds(1500));

        // Assert
        isCalled.Should().BeTrue();
    }
}
