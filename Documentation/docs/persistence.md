Schedule state is lost on application exit, there's no support for persisting it on disk. Since schedules are not serializable, implementing on your own is unadvised.
