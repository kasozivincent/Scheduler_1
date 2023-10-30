namespace Scheduler_1.Tests;

[TestFixture]
public class DailyScheduleTests
{
     [Test]
    public void CalculateDetails_RecurringDailyScheduleEnabled_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            StartDate = new DateTime(2020, 1, 1),
            CurrentDate = new DateTime(2020, 5, 5),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Daily,
            EndDate = new DateTime(2020, 10, 1)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2020, 5, 6)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringDailyScheduleEnabledNoEndDate_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            StartDate = new DateTime(2020, 1, 1),
            CurrentDate = new DateTime(2020, 5, 5),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Daily,
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2020, 5, 6)));
            Assert.That(details.Description, Is.EqualTo($"Next schedule will execute on {details.NextDate}"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringDailyScheduleDisabled_ReturnsErrorMessage()
    {
        var configuration = new ScheduleConfiguration
        {
            StartDate = new DateTime(2020, 1, 1),
            CurrentDate = new DateTime(2020, 5, 5),
            Enabled = false,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Daily,
            EndDate = new DateTime(2020, 10, 1)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule was canceled"));
        });
    }
    [Test]
    public void CalculateDetails_RecurringDailyScheduleExpired_ReturnsInvalidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 10, 5),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Daily,
            EndDate = new DateTime(2023, 10, 1)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule can't be executed"));
        });
    }
}