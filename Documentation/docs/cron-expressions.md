Thanks to [NCronTab](https://github.com/atifaziz/NCronTab), you can set up schedules using cron expressions:

```cs
var schedule = new Schedule(() => Console.WriteLine("5 minutes just passed."), "*/5 * * * *");

schedule.Start();
```

[Please refer to its documentation for expression examples](https://github.com/atifaziz/NCrontab/wiki/Crontab-Examples).