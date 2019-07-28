You can group multiple schedules in a collection and manage them all at once:

```cs
var s1 = new Schedule(() => Console.WriteLine("A minute just passed."), run => run.Every(1).Minutes());
var s2 = new Schedule(() => Console.WriteLine("5 minutes just passed."), run => run.Every(5).Minutes());
var s3 = new Schedule(() => Console.WriteLine("10 minutes just passed."), run => run.Every(10).Minutes());

var schedules = new[] { s1, s2, s3, };

schedules.Start();
```

Note that there's no other objects involved here other than the schedules and your collection, managing the group is
done through extension methods.

You should be able to do pretty much everything you can do with a single schedule.