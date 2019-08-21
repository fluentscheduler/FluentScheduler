By default, schedules uses local time.

Call `UseUtc` to change the schedule to UTC standard. 

```cs
var schedule = new Schedule(() => { }, run => run.Now());

schedule.UseUtc();
schedule.Start();
```

**Do not call `UseUtc` after `Start` without calling `Stop` first or current timezone is still going to be used.**


```cs
var schedule = new Schedule(() => { }, run => run.Now());

schedule.Start();

schedule.StopAndBlock();

schedule.UseUtc();

schedule.Start();
```