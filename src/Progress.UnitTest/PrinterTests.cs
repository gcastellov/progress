using Progress.Descriptors;
using Progress.Reporters;

namespace Progress.UnitTest;

public class PrinterTests
{
    [Fact]
    public void GivenDefaults_WhenPrinting_ThenGetsOutput()
    {
        // Arrange
        Printer printer = new(new Settings.Console.ReportingOptions(), [ Workload.Default(10, BarDescriptor.Default.Build()) ]);
        Stats stats = new();

        // Act
        var actual = printer.Print(stats);

        // Assert
        actual.Should().NotBeNullOrEmpty();
    }
}
