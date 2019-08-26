Every 2 seconds:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(2).Seconds());
```

Once, after 5 seconds:

```cs
new Schedule(() => { /* your code */ }, run => run.OnceIn(5).Seconds());
```

Now and every 5 minutes afterwards:

```cs
new Schedule(() => { /* your code */ }, run => run.Now().AndEvery(5).Minutes());
```

Every weekday:

```cs
new Schedule(() => { /* your code */ }, run => run.EveryWeekDay())
```

Every weekend:
```cs
new Schedule(() => { /* your code */ }, run => run.EveryWeekend())
```

Everyday at 21:15:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(1).Days().At(21, 15));
```

Every 20th on the month:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(1).Month().On(20))
```

Everyday except Mondays:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(1).Days().Except(DayOfWeek.Monday))
```

Everyday between 01:00 and 04:00:

```cs
new Schedule(() => { /* your code */ }, run => run.Now().AndEvery(1).Days().Between(1, 0, 4, 0))
```

Every first Monday of the month, at 03:00:

```cs
new Schedule(() => { /* your code */ }, run => run.Now().AndEvery(1).Months().OnTheFirstDay(DayOfWeek.Monday).At(3, 0));
```