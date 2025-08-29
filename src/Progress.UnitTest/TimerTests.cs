namespace Progress.UnitTest;

public class TimerTests
{
    [Fact]
    public void GivenNothingDone_WhenGettingRemainingTime_ThenReturnsMaxValue()
    {
        // Arrange
        var timer = Timer.Start();

        // Act
        var actual = timer.GetRemainingTime(0);

        // Assert
        actual.Should().Be(TimeSpan.MaxValue);
    }


    [Fact]
    public void GivenEverythingDone_WhenGettingRemainingTime_ThenReturnsZero()
    {
        // Arrange
        var timer = Timer.Start();

        // Act
        var actual = timer.GetRemainingTime(100);

        // Assert
        actual.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void GivenNothingDone_WhenGettingEstimatedTimeOfArrival_ThenReturnsMaxValue()
    {
        // Arrange
        var timer = Timer.Start();

        // Act
        var actual = timer.GetEstimatedTimeOfArrival(0);

        // Assert
        actual.Should().Be(DateTimeOffset.MaxValue);
    }

    [Fact]
    public void GivenInstance_WhenGettingStartedOn_ThenReturnsMomentOfCreation()
    {
        // Arrange
        var moment = DateTimeOffset.UtcNow;
        var timer = new Timer(moment);

        // Act
        var actual = timer.StartedOn;

        // Assert
        actual.Should().Be(moment);
    }

    [Fact]
    public async Task GivenSomeWorkDone_WhenGettingElapsedTime_ThenReturnsTime()
    {
        // Arrange
        var timer = Timer.Start();
        var delay = TimeSpan.FromSeconds(1);
        await Task.Delay(delay);

        // Act
        var actual = timer.ElapsedTime;

        // Assert
        actual.Should().BeGreaterThanOrEqualTo(delay);
    }
}
