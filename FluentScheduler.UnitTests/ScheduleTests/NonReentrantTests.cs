namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NonReentrantTests
    {
        [TestMethod]
        public void Should_Be_Null_By_Default()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow();

            // Assert
            Assert.IsNull(schedule.Reentrant);
        }

        [TestMethod]
        public void Should_Default_Reentrant_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsNull(schedule.Reentrant);

            foreach (var child in schedule.AdditionalSchedules)
                Assert.IsNull(child.Reentrant);
        }

        [TestMethod]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsNotNull(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.AreEqual(schedule.Reentrant, child.Reentrant);
        }
    }
}
