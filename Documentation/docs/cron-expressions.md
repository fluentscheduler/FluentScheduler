Thanks to [NCronTab](https://github.com/atifaziz/NCronTab), you can set up schedules using cron expressions:

```cs
var schedule = new Schedule(
    () => Console.WriteLine("5 minutes just passed."),
    "*/5 * * * *"
);

schedule.Start();
```

NCronTab also support 
[merging schedules](https://github.com/atifaziz/NCrontab/tree/master?tab=readme-ov-file#merging-schedules) which allows 
a deeper level of control over the execution of your tasks. 
The following cron expressions will execute the task at midnight on the given day of the month.

```cs
List<string> cronExpressions =
[
    "0 0 31 1,3,5,7,8,10,12 *",
    "0 0 30 4,6,9,11 *",
    "0 0 28 2 *",
];

var schedule = new Schedule(() => Console.WriteLine("5 minutes just passed."), cronExpressions);
schedule.Start();
```

[Please refer to its documentation for expression examples](https://github.com/atifaziz/NCrontab/wiki/Crontab-Examples).