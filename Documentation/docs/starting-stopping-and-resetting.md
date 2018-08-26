
Starting:

```cs
schedule.Start();
```

Stopping:

```cs
schedule.Stop();
```

This call doesn't block, in other words, it returns immediatelly without waiting for your action to stop.

Stopping and blocking until the action ends:

```cs
schedule.StopAndBlock();
```