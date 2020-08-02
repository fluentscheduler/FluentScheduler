namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Xunit;

    public class NonReentrantTests
    {
        [Fact]
        public void Should_Be_Null_By_Default()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow();

            // Assert
            Assert.Null(schedule.Reentrant);
        }

        [Fact]
        public void Should_Default_Reentrant_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.Null(schedule.Reentrant);

            foreach (var child in schedule.AdditionalSchedules)
                Assert.Null(child.Reentrant);
        }

        [Fact]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.NotNull(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.Equal(schedule.Reentrant, child.Reentrant);
        }
    }
}
