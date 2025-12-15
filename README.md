<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="https://raw.githubusercontent.com/fluentscheduler/FluentScheduler/version-6/Logo/logo-200x200.png">
    </a>
</p>

<p align="center">
    <a href="https://github.com/fluentscheduler/FluentScheduler/actions/workflows/build.yml">
        <img alt="badge" src="https://img.shields.io/github/actions/workflow/status/fluentscheduler/fluentscheduler/build.yml?logo=github&branch=version-5">
    </a>
    <a href="https://www.nuget.org/packages/FluentScheduler">
        <img alt="badge" src="https://img.shields.io/nuget/dt/FluentScheduler.svg?logo=nuget">
    </a>
</p>

# FluentScheduler

**Important: this documentation refers to the version 5 of the library which is currently deprecated.
Check the current version [here](https://github.com/fluentscheduler/FluentScheduler).**

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
[documentation]: https://fluentscheduler.github.io/v5
[issues]:        https://github.com/fluentscheduler/FluentScheduler/issues
[help wanted]:   https://github.com/fluentscheduler/FluentScheduler/labels/help%20wanted
