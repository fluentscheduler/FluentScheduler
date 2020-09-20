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

Automated job scheduler with fluent interface for the .NET platform.

```cs
JobManager.Initialize();

JobManager.AddJob(
    () => Console.WriteLine("5 minutes just passed."),
    s => s.ToRunEvery(5).Minutes()
);
```

**Learning?**
Check the [documentation]!

**Comments? Problems? Suggestions?**
Check the [issues]!

**Want to help?**
Check the [help wanted] label!

[master branch]: https://github.com/fluentscheduler/FluentScheduler
[documentation]: http://fluentscheduler.github.io
[issues]:        https://github.com/fluentscheduler/FluentScheduler/issues
[help wanted]:   https://github.com/fluentscheduler/FluentScheduler/labels/help%20wanted
