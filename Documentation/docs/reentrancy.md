All schedules are non-reentrant, that means you won't have multiple executions of the same schedule running at the same time.

For instance, the schedule below runs 2x after 10 seconds, and not 10x as scheduled:

```cs
var schedule = new Schedule(
    () =>
    {
        Console.WriteLine("Waiting 5 seconds...");
        Thread.Sleep(5000);
    },
    run => run.Every(1).Seconds()
);

schedule.Start();
```
