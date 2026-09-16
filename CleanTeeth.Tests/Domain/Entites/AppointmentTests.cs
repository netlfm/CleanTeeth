using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
namespace CleanTeeth.Tests.Domain.Entites;

[TestClass]
public class AppointmentTests
{
    private Guid _patientId = Guid.NewGuid();
    private Guid _dentistId = Guid.NewGuid();
    private Guid _dentalOfficeId = Guid.NewGuid();
    private TimeInterval _interval = new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));

    [TestMethod]
    public void Constructor_ValidAppointment_StatusIsScheduled()
    {
        // Act: 执行被测行为
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);

        // Assert: 验证聚合根的状态是否符合预期
        appointment.Should().BeEquivalentTo(new
        {
            PatientId = _patientId,
            DentistId = _dentistId,
            DentalOfficeId = _dentalOfficeId,
            Status = AppointmentStatus.Scheduled,
            TimeInterval = _interval
        });

        // Assert: 验证实体标识是否已正确生成
        appointment.Id.Should().NotBeEmpty();
    }
    [TestMethod]
    public void Constructor_StartTimeInThePast_Throws()
    {
        // Arrange: 准备违反业务规则的数据（开始时间在过去）
        var pastInterval = new TimeInterval(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);

        // Act: 将构造函数调用包装为 Action 委托（延迟执行）
        Action act = () => new Appointment(_patientId, _dentistId, _dentalOfficeId, pastInterval);

        // Assert: 断言执行该委托会抛出特定的业务规则异常
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Cancel_CancellingAppointment_ChangesStatusToCancelled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        appointment.Status.Should().Be(AppointmentStatus.Cancelled);
    }
    [TestMethod]
    public void Cancel_CancellingAppointment_ThrowsIfStatusIsNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        Action act = () => appointment.Cancel(); // This should throw
        act.Should().Throw<BusinessRuleException>();
    }
    [TestMethod]
    public void Complete_CompletingAppointment_ChangesStatusToCompleted()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Complete();
        appointment.Status.Should().Be(AppointmentStatus.Completed);
    }
    [TestMethod]
    public void Complete_CompletingAppointment_ThrowsIfStatusIsNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        Action act = () => appointment.Complete(); // This should throw
        act.Should().Throw<BusinessRuleException>();
    }
}
