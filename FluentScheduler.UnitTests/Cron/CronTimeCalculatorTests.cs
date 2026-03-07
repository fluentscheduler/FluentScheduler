using System.Collections.Generic;

namespace FluentScheduler.UnitTests;

using System;
using Xunit;
using static Xunit.Assert;

public class CronTimeCalculatorTests
{
    [Fact]
    public void At0405()
    {
        // Arrange
        var calculator = new CronTimeCalculator("5 4 * * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 22, 4, 5, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At0005InAugust()
    {
        // Arrange
        var calculator = new CronTimeCalculator("5 0 * 8 *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2019, 8, 1, 0, 5, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At0405OnSunday()
    {
        // Arrange
        var calculator = new CronTimeCalculator("5 4 * * sun");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 23, 4, 5, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At0400OnEveryDayOfMonthFrom8Through14()
    {
        // Arrange
        var calculator = new CronTimeCalculator("0 4 8-14 * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2019, 01, 08, 4, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At1415OnJanuaryFirst()
    {
        // Arrange
        var calculator = new CronTimeCalculator("15 14 1 * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2019, 1, 1, 14, 15, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At2200OnEveryDayOfWeekFromMondayThroughFriday()
    {
        // Arrange
        var calculator = new CronTimeCalculator("0 22 * * 1-5");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 24, 22, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EveryDayAt0500And1700Hours()
    {
        // Arrange
        var calculator = new CronTimeCalculator("0 5,17 * * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 22, 5, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void AtEveryMinute()
    {
        // Arrange
        var calculator = new CronTimeCalculator("* * * * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 22, 0, 1, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void AtEvery10Minutes()
    {
        // Arrange
        var calculator = new CronTimeCalculator("*/10 * * * *");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 22, 0, 10, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void At1700OnSundayAndFriday()
    {
        // Arrange
        var calculator = new CronTimeCalculator("0 17 * * sun,fri");

        var date = new DateTime(2018, 12, 22);
        var expected = new DateTime(2018, 12, 23, 17, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EverySecond()
    {
        // Arrange
        var calculator = new CronTimeCalculator("* * * * * *");

        var date = new DateTime(2018, 12, 23, 17, 0, 0);
        var expected = new DateTime(2018, 12, 23, 17, 0, 1);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EveryLastDayOfTheMonth()
    {
        List<string> cronExpressions =
        [
            "0 0 31 1,3,5,7,8,10,12 *",
            "0 0 30 4,6,9,11 *",
            "0 0 28 2 *",
            "0 0 29 2 *"
        ];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 02, 20);
        var expected = new DateTime(2026, 02, 28, 0, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        calculated = calculator.Calculate(expected);

        expected = new DateTime(2026, 03, 31, 0, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void InLeapYearShouldRun28And29February()
    {
        List<string> cronExpressions =
        [
            "0 0 31 1,3,5,7,8,10,12 *",
            "0 0 30 4,6,9,11 *",
            "0 0 28 2 *",
            "0 0 29 2 *"
        ];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2024, 02, 20);
        var expected = new DateTime(2024, 02, 28, 0, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(expected);

        // Assert
        expected = new DateTime(2024, 02, 29, 0, 0, 0);
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(expected);

        // Assert
        expected = new DateTime(2024, 03, 31, 0, 0, 0);
        Equal(expected, calculated);
    }

    [Fact]
    public void EveryDayAtMidnight()
    {
        List<string> cronExpressions = ["0 0 * * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 06, 10, 30, 0);
        var expected = new DateTime(2026, 03, 07, 0, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 03, 08, 0, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EveryDayAtNoon()
    {
        List<string> cronExpressions = ["0 12 * * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 06, 8, 0, 0);
        var expected = new DateTime(2026, 03, 06, 12, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 03, 07, 12, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void FirstDayOfEveryMonth()
    {
        List<string> cronExpressions = ["0 0 1 * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 15, 9, 0, 0);
        var expected = new DateTime(2026, 04, 01, 0, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 05, 01, 0, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void FirstDayOfEveryMonthAt5Am()
    {
        List<string> cronExpressions = ["0 5 1 * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 15, 9, 0, 0);
        var expected = new DateTime(2026, 04, 01, 5, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 05, 01, 5, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void FifthDayOfEveryMonth()
    {
        List<string> cronExpressions = ["0 0 5 * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 01, 14, 0, 0);
        var expected = new DateTime(2026, 03, 05, 0, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 04, 05, 0, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EverydayAt8And16()
    {
        List<string> cronExpressions = ["0 8,16 * * *"];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 06, 9, 0, 0);
        var expected = new DateTime(2026, 03, 06, 16, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 03, 07, 8, 0, 0);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void EverydayAt8And16Merging()
    {
        List<string> cronExpressions =
        [
            "0 8 * * *",
            "0 16 * * *"
        ];

        // Arrange
        var calculator = new CronTimeCalculator(cronExpressions);

        var date = new DateTime(2026, 03, 06, 9, 0, 0);
        var expected = new DateTime(2026, 03, 06, 16, 0, 0);

        // Act
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);

        // Act
        calculated = calculator.Calculate(calculated!.Value);
        expected = new DateTime(2026, 03, 07, 8, 0, 0);

        // Assert
        Equal(expected, calculated);
    }
}