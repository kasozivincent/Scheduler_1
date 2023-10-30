namespace Scheduler_1.Tests;

[TestFixture]
public class OnceScheduleTests
{
    [Test]
    public void CalculateDetails_OnceScheduleEnabled_ReturnsValidDetails()
    {
        var currentDate = new DateTime(2023, 10, 1);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = true,
            ScheduleType = ScheduleType.Once,
            ExecutionDate = new DateTime(2023, 10, 5)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(new DateTime(2023, 10, 5)));
            Assert.That(details.Description, Is.EqualTo("Schedule will execute on 05/10/2023 00:00:00"));
        });
    }
    [Test]
    public void CalculateDetails_OnceScheduleDisabled_ReturnsCancellationDetails()
    {
        var currentDate = new DateTime(2023, 10, 1);
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = currentDate,
            Enabled = false,
            ScheduleType = ScheduleType.Once,
            ExecutionDate = new DateTime(2023, 10, 5)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule was canceled"));
        });
    }
    [Test]
    public void CalculateDetails_OnceScheduleExpired_ReturnsErrorDetails()
    {
        var configuration = new ScheduleConfiguration
        {
            CurrentDate = new DateTime(2023, 10, 1),
            Enabled = true,
            ScheduleType = ScheduleType.Once,
            ExecutionDate = new DateTime(2020, 10, 5)
        };

        var details = Scheduler.CalculateDetails(configuration);
        Assert.Multiple(() =>
        {
            Assert.That(details.NextDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(details.Description, Is.EqualTo("Schedule can't be executed"));
        });
    }
}