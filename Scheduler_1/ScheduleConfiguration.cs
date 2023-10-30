namespace Scheduler_1;

public enum ScheduleType
{
    Once,
    Recurring
}

public enum Occurs
{
    Daily,
    Weekly,
    Monthly
}

public record ScheduleDetails(DateTime NextDate, string Description);

public class ScheduleConfiguration
{
    public DateTime CurrentDate { get; set; }
    public bool Enabled { get; set; }
    public ScheduleType ScheduleType { get; set; }
    public DateTime ExecutionDate { get; set; }
    public Occurs Occurs { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class Scheduler
{
    public static ScheduleDetails CalculateDetails(ScheduleConfiguration configuration)
    {
        if (configuration.ScheduleType == ScheduleType.Once)
            return OnceScheduleDetails(configuration);
        return configuration.Occurs switch
        {
            Occurs.Daily => RecurringDailyScheduleDetails(configuration),
            Occurs.Weekly => RecurringWeeklyScheduleDetails(configuration),
            _ => RecurringMonthlyScheduleDetails(configuration)
        };
    }
    private static ScheduleDetails RecurringMonthlyScheduleDetails(ScheduleConfiguration configuration)
    {
        if (!configuration.Enabled)
            return new ScheduleDetails(DateTime.MinValue, "Schedule was canceled");
        if (configuration.EndDate != null && configuration.EndDate < configuration.CurrentDate)
            return new ScheduleDetails(DateTime.MinValue, "Schedule can't be executed");
        DateTime date;
        if (configuration.CurrentDate.Day >= configuration.StartDate.Day)
        {
            var a = configuration.CurrentDate.AddMonths(1);
            date = new DateTime(a.Year, a.Month, configuration.StartDate.Day);
            return new ScheduleDetails(date, $"Next schedule will execute on {date.Date}");
        }

        date = new DateTime(configuration.CurrentDate.Year, configuration.CurrentDate.Month,
            configuration.StartDate.Day);
        return new ScheduleDetails(date, $"Next schedule will execute on {date}");
    }
    private static ScheduleDetails RecurringWeeklyScheduleDetails(ScheduleConfiguration configuration)
    {
        if (!configuration.Enabled)
            return new ScheduleDetails(DateTime.MinValue, "Schedule was canceled");
        else if (configuration.EndDate != null && configuration.EndDate < configuration.CurrentDate)
            return new ScheduleDetails(DateTime.MinValue, "Schedule can't be executed");
        else
        {
            var date = CalculateNextScheduleDate(configuration.StartDate, configuration.CurrentDate);
            return new ScheduleDetails(date,$"Next schedule will execute on {date.Date}");
        }
    }
    
    private static DateTime CalculateNextScheduleDate(DateTime startDate, DateTime currentDate)
    {
        if (currentDate.DayOfWeek == startDate.DayOfWeek)
        {
            return currentDate.AddDays(7);
        }
        else if (currentDate.DayOfWeek < startDate.DayOfWeek)
        {
            int daysToAdd = (int)startDate.DayOfWeek - (int)currentDate.DayOfWeek;
            return currentDate.AddDays(daysToAdd);
        }
        else
        {
            int daysToAdd = 7 - (int)currentDate.DayOfWeek + (int)startDate.DayOfWeek;
            return currentDate.AddDays(daysToAdd);
        }
    }

    private static ScheduleDetails RecurringDailyScheduleDetails(ScheduleConfiguration configuration)
    {
        if (!configuration.Enabled)
            return new ScheduleDetails(DateTime.MinValue, "Schedule was canceled");
        else if (configuration.EndDate != null && configuration.EndDate < configuration.CurrentDate)
            return new ScheduleDetails(DateTime.MinValue, "Schedule can't be executed");
        else
            return new ScheduleDetails(configuration.CurrentDate.AddDays(1),
                $"Next schedule will execute on {configuration.CurrentDate.AddDays(1)}");
    }
    private static ScheduleDetails OnceScheduleDetails(ScheduleConfiguration configuration)
    {
        if (!configuration.Enabled)
            return new ScheduleDetails(DateTime.MinValue, "Schedule was canceled");
        else if (configuration.ExecutionDate < configuration.CurrentDate)
            return new ScheduleDetails(DateTime.MinValue, "Schedule can't be executed");
        else
            return new ScheduleDetails(configuration.ExecutionDate,
                $"Schedule will execute on {configuration.ExecutionDate}");
    }
}