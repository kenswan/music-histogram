namespace BlazorMusic.Client.Provider;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetDateTimeNow() => DateTime.Now;
}
