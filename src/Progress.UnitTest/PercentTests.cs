using Progress.Components;

namespace Progress.UnitTest;

public class PercentTests
{
    [Theory]
    [InlineData(0, "0.00%")]
    [InlineData(1, "1.00%")]
    [InlineData(3, "3.00%")]
    [InlineData(50, "50.00%")]
    [InlineData(75, "75.00%")]
    [InlineData(100, "100.00%")]
    [InlineData(120, "120.00%")]
    public void GivenPercent_WhenDisplaying_ReturnsProperOutput(ulong count, string expectedOutput)
    {
        // Arrange
        var percent = new Component.Percent(100, count);

        // Act
        string actual = percent.ToString();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(50, true)]
    [InlineData(75, true)]
    [InlineData(100, false)]
    [InlineData(120, false)]
    public void GivenPercent_WhenCheckingIfInRange_ReturnsExpected(ulong count, bool expected)
    {
        // Arrange
        var percent = new Component.Percent(100, count);

        // Act
        bool actual = percent.IsInRange;

        // Assert
        actual.Should().Be(expected);
    }
}