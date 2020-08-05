<p align="center">
    <img alt="logo" src="https://raw.githubusercontent.com/fluentscheduler/FluentScheduler/version-6/Logo/logo-200x200.png">
</p>

**Important note**: This documentation refers to the upcoming version of the library, that's still unstable. For the current version please head to [GitHub](https://github.com/fluentscheduler/FluentScheduler).


Welcome to the documentation for FluentScheduler, an automated job scheduler with fluent interface for the .NET
platform.

```cs
var schedule = new Schedule(
    () => Console.WriteLine("5 minutes just passed."),
    run => run.Every(5).Minutes()
);

schedule.Start();
```

Make sure to check the [GitHub repository](https://github.com/fluentscheduler/FluentScheduler) for community discussion
and its source code.
