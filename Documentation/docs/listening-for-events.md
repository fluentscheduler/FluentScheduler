Listening for the job start:

```cs
schedule.JobStarted += (sender, ea) =>
{
    Console.WriteLine($"Job started at {ea.StartTime}.");
};
```

Listening for the job end:

```cs
schedule.JobEnded += (sender, ea) =>
{
    if (ea.Exception == null)
        Console.WriteLine($"The job ended at {ea.EndTime}.")
    else
        Console.WriteLine($"The job ended at {ea.EndTime} due to an error: {ea.Exception}");

    if (ea.NextRun.HasValue)
        Console.WriteLine($"Next run at {ea.NextRun.Value}.");
};
```