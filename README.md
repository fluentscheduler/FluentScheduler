<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="https://raw.githubusercontent.com/fluentscheduler/FluentScheduler/version-6/Logo/logo-200x200.png">
    </a>
</p>

<p align="center">
    <a href="https://ci.appveyor.com/project/TallesL/fluentscheduler">
        <img alt="logo" src="https://ci.appveyor.com/api/projects/status/github/fluentscheduler/fluentscheduler?svg=true">
    </a>
    <a href="https://www.nuget.org/packages/FluentScheduler">
        <img alt="logo" src="https://badge.fury.io/nu/fluentscheduler.svg">
    </a>
</p>

# FluentScheduler

**Important note**: This branch refers to the upcoming version of the library, that's still unstable. For the current version please head to the [master branch].

Automated job scheduler with fluent interface for the .NET platform.

```cs
var schedule = new Schedule(
    () => Console.WriteLine("5 minutes just passed."),
    run => run.Every(5).Minutes()
);

schedule.Start();
```

**Learning?**
Check the [documentation]!

**Comments? Problems? Suggestions?**
Check the [issues]!

**Want to help?**
Check the [help wanted] label!

[master branch]: https://github.com/fluentscheduler/FluentScheduler
[documentation]: http://fluentscheduler.github.io/v6
[issues]:        https://github.com/fluentscheduler/FluentScheduler/issues
[help wanted]:   https://github.com/fluentscheduler/FluentScheduler/labels/help%20wanted
