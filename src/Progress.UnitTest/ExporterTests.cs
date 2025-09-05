using Progress.Settings;

namespace Progress.UnitTest;

public class ExporterTests
{
    private readonly Stats _stats = new()
    {
        StartedOn = DateTimeOffset.Now.AddMinutes(-1),
        CurrentCount = 1000,
        CurrentPercent = 100,
        ElapsedTime = TimeSpan.FromMinutes(1),
        EstTimeOfArrival = DateTimeOffset.UtcNow,
        ExpectedItems = 1000,
        FailureCount = 30,
        SuccessCount = 970,
        RemainingTime = TimeSpan.Zero
    };


    [Theory]
    [InlineData("output.csv", FileType.Csv)]
    [InlineData("output.json", FileType.Json)]
    [InlineData("output.txt", FileType.Text)]
    public void GivenStats_WhenExporting_ThenExportsSuccessfully(string fileName, FileType fileType)
    {
        // Arrange
        ExportSettings settings = new(fileName, fileType);
        Exporter exporter = new(settings);

        // Act
        exporter.Export(_stats);

        // Assert
        File.Exists(fileName).Should().BeTrue();
    }
}
