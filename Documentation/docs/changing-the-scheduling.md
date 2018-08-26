Resetting the scheduling:

```cs
schedule.ResetScheduling();
```

Changing the scheduling:

```cs
schedule.SetScheduling(run => run.Every(2).Hour());
```

You can't change the scheduling if the schedule is already running, make sure it's stopped before doing so.