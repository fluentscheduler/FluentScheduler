Just instantiate a new schedule, giving the job to run and when it should run, starts it and it's running:

```cs
var schedule = new Schedule(
    () => Console.WriteLine("5 minutes just passed."),
    run => run.Every(5).Minutes()
);

schedule.Start();
```