By default, schedules uses local time.

Call `UseUtc` to change the schedule to UTC standard. 

```cs
var schedule = new Schedule(() => { }, run => run.Now());

schedule.UseUtc();
schedule.Start();
```

**OBS: Do not call `UseUtc` after `Start`, nothing is going to happen.**
