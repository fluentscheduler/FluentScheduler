namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class SpecificRunTimeTests
    {
        [TestMethod]
        public void Should_Add_Chained_Jobs_To_AdditionalSchedules_Property()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Months();

            // Assert
            Assert.AreEqual(1, schedule.AdditionalSchedules.Count);
        }

        [TestMethod]
        public void Should_Set_Chained_Job_Schedule_As_Expected()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 3, 1);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(2).Months();
            var actual = schedule.AdditionalSchedules.ElementAt(0).CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Not_Alter_Original_Runtime_If_Chained_Job_Exists()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Months();

            // Assert
            Assert.IsNull(schedule.CalculateNextRun);
        }
    }
}
