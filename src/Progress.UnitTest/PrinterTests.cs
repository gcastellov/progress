using Progress.Descriptors;

namespace Progress.UnitTest;

public class PrinterTests
{
    [Fact]
    public void GivenDefaults_WhenPrinting_ThenGetsOutput()
    {
        // Arrange
        Printer printer = new(new Settings.Console.ReportingOptions(), BarDescriptor.Default.Build());
        Stats stats = new();

        // Act
        var actual = printer.Print(stats);

        // Assert
        actual.Should().NotBeNullOrEmpty();
    }
}
