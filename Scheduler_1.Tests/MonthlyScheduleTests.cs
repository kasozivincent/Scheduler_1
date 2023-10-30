namespace Scheduler_1.Tests;

public class MonthlyScheduleTests
{
     [Test]
    public void CalculateDetails_RecurringMonthlyScheduleEnabledBeforeDate_ReturnsValidDetails()
    {
        var currentDate = new DateTime(2020, 10, 1);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2020, 1, 6),
            EndDate = new DateTime(2020, 12, 31)
        };
        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2020, 10, 6)));
            Assert.That(details.Description, Is.EqualTo("Next schedule will execute on 06/10/2020 00:00:00"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringMonthlyScheduleEnabledExactDate_ReturnsValidDetails()
    {
        var currentDate = new DateTime(2020, 10, 6);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2020, 1, 6),
            EndDate = new DateTime(2020, 12, 31)
        };
        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2020, 11, 6)));
            Assert.That(details.Description, Is.EqualTo("Next schedule will execute on 06/11/2020 00:00:00"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringMonthlyScheduleEnabledAfterDate_ReturnsValidDetails()
    {
        var currentDate = new DateTime(2020, 10, 10);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2020, 1, 6),
            EndDate = new DateTime(2020, 12, 31)
        };
        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2020, 11, 6)));
            Assert.That(details.Description, Is.EqualTo("Next schedule will execute on 06/11/2020 00:00:00"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringMonthlyScheduleDisabled_CancellationDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2020, 10, 10),
            Enabled = false,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2020, 1, 6),
            EndDate = new DateTime(2020, 12, 31)
        };
        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule was canceled"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringMonthlyScheduleEnabledNoEndDate_ReturnsValidDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 10, 1),
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2023, 10, 15)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 10, 15)));
            Assert.That(details.Description, Is.EqualTo("Next schedule will execute on 15/10/2023 00:00:00"));
        });
    }
    
    [Test]
    public void CalculateDetails_RecurringMonthlyScheduleEnabledExpiredDate_ReturnsErrorDetails()
    {
        var currentDate = new DateTime(2023, 10, 1);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = true,
            ScheduleType = ScheduleType.Recurring,
            Occurs = Occurs.Monthly,
            StartDate = new DateTime(2020, 1, 15),
            EndDate = new DateTime(2020, 10, 15)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule can't be executed"));
        });
    }
    
}