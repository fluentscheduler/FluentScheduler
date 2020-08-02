namespace FluentScheduler.UnitTests
{
    using Xunit;
    using static Xunit.Assert;

    public class NonReentrantTests
    {
        [Fact]
        public void Should_Be_Null_By_Default()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow();

            // Assert
            Null(schedule.Reentrant);
        }

        [Fact]
        public void Should_Default_Reentrant_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Null(schedule.Reentrant);

            foreach (var child in schedule.AdditionalSchedules)
                Null(child.Reentrant);
        }

        [Fact]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            NotNull(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Equal(schedule.Reentrant, child.Reentrant);
        }
    }
}
