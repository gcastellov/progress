using Progress.Components;
using Progress.Descriptors;
using Progress.Reporters;

namespace Progress.UnitTest;

public class PrinterTests
{
    [Fact]
    public void GivenDefaults_WhenPrinting_ThenGetsOutput()
    {
        // Arrange
        var workload = Workload.Default(10);
        workload.Component = BarDescriptor.Default.Build();
        Printer printer = new(new Settings.Console.ReportingOptions(), [ workload ]);
        Stats stats = new();

        // Act
        var actual = printer.Print(stats);

        // Assert
        actual.Should().NotBeNullOrEmpty();
    }
}
