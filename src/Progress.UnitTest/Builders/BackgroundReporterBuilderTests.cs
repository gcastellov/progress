using Progress.Builders;
using Progress.Settings;

namespace Progress.UnitTest.Builders;

public class BackgroundReporterBuilderTests
{
    [Fact]
    public void GivenProgressNotifications_WhenBuilding_ThenReporterIsSetUp()
    {
        // Arrange
        var callback = (Stats stats) => { };
        var builder = new BackgroundReporterBuilder()
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
        var builder = new BackgroundReporterBuilder()
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
        var builder = new BackgroundReporterBuilder()
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
        var builder = new BackgroundReporterBuilder()
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
