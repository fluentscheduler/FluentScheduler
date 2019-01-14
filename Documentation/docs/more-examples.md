Every 2 seconds:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(2).Seconds());
```

Once, after 5 seconds:

```cs
new Schedule(() => { /* your code */ }, run => run.OnceIn(5).Seconds());
```

Everyday at 21:15:

```cs
new Schedule(() => { /* your code */ }, run => run.Every(1).Days().At(21, 15));
```

Now and every 5 minutes afterwards:

```cs
new Schedule(() => { /* your code */ }, run => run.Now().AndEvery(5).Minutes());
```

Every first Monday of the month, at 03:00:

```cs
new Schedule(() => { /* your code */ }, run => run.Now().AndEvery(1).Months().OnTheFirstDay(DayOfWeek.Monday).At(3, 0));
```