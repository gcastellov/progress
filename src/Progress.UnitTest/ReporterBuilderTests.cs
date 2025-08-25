namespace Progress.UnitTest;

public class ReporterBuilderTests
{
    [Fact]
    public void GivenNothingToComplete_WhenBuilding_ThenThrowsException()
    {
        // Arrange
        var builder = new ReporterBuilder();

        // Act
        var action = () => builder.Build(0);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenSomethingToComplete_WhenBuilding_ThenReturnsReporter()
    {
        // Arrange
        var builder = new ReporterBuilder();

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
        var builder = new ReporterBuilder()
            .UsingReportingFrequency(expectedFrequency);

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.ReportFrequency.Should().Be(expectedFrequency);
    }

    [Fact]
    public void GivenElapsedTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ReporterBuilder()
            .DisplayingElapsedTime();

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.DisplayElapsedTime.Should().BeTrue();
    }

    [Fact]
    public void GivenStartingTime_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ReporterBuilder()
            .DisplayingStartingTime();

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.DisplayStartingTime.Should().BeTrue();
    }

    [Fact]
    public void GivenItemsOverview_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ReporterBuilder()
            .DisplayingItemsOverview();

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.DisplayItemsOverview.Should().BeTrue();
    }

    [Fact]
    public void GivenSummary_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var builder = new ReporterBuilder()
            .DisplayingItemsSummary();

        // Act
        var actual = builder.Build(100);

        // Assert
        actual.DisplayItemsSummary.Should().BeTrue();
    }
}
