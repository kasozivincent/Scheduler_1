namespace Scheduler_1.Tests;

public class WeeklyScheduleTests
{
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleEnabled_DayEqualToStartDateDay_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 12, 2),
            StartDate = new DateTime(2023, 11, 4),
            EndDate = new DateTime(2024, 4, 1),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 12, 9)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleEnabled_DayBefore_StartDateDay_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 12, 14),
            StartDate = new DateTime(2023, 11, 4),
            EndDate = new DateTime(2024, 4, 1),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 12, 16)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleEnabled_DayAfter_StartDateDay_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 12, 17),
            StartDate = new DateTime(2023, 11, 4),
            EndDate = new DateTime(2024, 4, 1),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 12, 23)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    
    
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleEnabledNoEndDate_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 12, 17),
            StartDate = new DateTime(2023, 11, 4),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 12, 23)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleDisabled_ReturnsCancelledDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 10, 1),
            Enabled = false,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule was canceled"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringWeeklyScheduleExpiredEndDate_ReturnsErrorDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 10, 1),
            Enabled = true,
            EndDate = new DateTime(2021, 1, 1),
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Weekly
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule can't be executed"));
        });
    }
}