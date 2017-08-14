<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" height="150px" src="https://raw.githubusercontent.com/fluentscheduler/FluentScheduler/logo/logo.png">
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

Automated job scheduler with fluent interface.

```cs
var schedule = new Schedule(
    () => Console.WriteLine("5 minutes just passed."),
    run => run.Now().AndEvery(5).Minutes()
);

schedule.Start();
```

**Learning?**
Check the [documentation]!

**Comments? Problems? Suggestions?**
Check the [issues]!

**Want to help?**
Check the ["help wanted"] label!

**Want to implement something yourself?**
Discuss it first in an [new] or [existing] issue, then open a [pull request]!

[documentation]: http://fluentscheduler.readthedocs.io
[issues]:        https://github.com/fluentscheduler/FluentScheduler/issues
["help wanted"]: https://github.com/fluentscheduler/FluentScheduler/labels/help%20wanted
[new]:           https://github.com/fluentscheduler/FluentScheduler/issues/new
[existing]:      https://github.com/fluentscheduler/FluentScheduler/issues
[pull request]:  https://github.com/fluentscheduler/FluentScheduler/pulls