There's support for async schedules:

```cs
var schedule = new Schedule(
    async () =>
    {
        using var client = new WebClient();
        var content = await client.DownloadStringTaskAsync("http://example.com");
        Console.WriteLine(content);
    },
    run => run.Now()
);
```
