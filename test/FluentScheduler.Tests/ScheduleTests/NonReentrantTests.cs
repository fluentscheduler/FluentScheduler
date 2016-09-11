namespace FluentScheduler.Tests.ScheduleTests
{
    using Xunit;

    public class NonReentrantTests
    {
        [Fact]
        public void Should_Be_True_By_Default()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow();

            // Assert
            Assert.True(schedule.Reentrant);
        }

        [Fact]
        public void Should_Default_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.True(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.True(child.Reentrant);
        }

        [Fact]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.False(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.False(child.Reentrant);
        }
    }
}
