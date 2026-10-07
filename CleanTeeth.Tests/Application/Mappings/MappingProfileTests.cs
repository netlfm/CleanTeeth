using AutoMapper;
using CleanTeeth.Application.Features.Appointments;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDetail;
using CleanTeeth.Application.Features.DentalOffices;
using CleanTeeth.Application.Features.Dentists;
using CleanTeeth.Application.Features.Patients;
using CleanTeeth.Application.Features.Treatments;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace CleanTeeth.Tests.Application.Mappings;

[TestClass]
public class PatientProfileTests
{
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Setup() =>
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<PatientProfile>(), NullLoggerFactory.Instance).CreateMapper();

    [TestMethod]
    public void Configuration_IsValid() =>
        new MapperConfiguration(cfg => cfg.AddProfile<PatientProfile>(), NullLoggerFactory.Instance).AssertConfigurationIsValid();

    [TestMethod]
    public void Map_MapsValueObjectValues()
    {
        var patient = TestData.NewPatient();

        var result = _mapper.Map<CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail.GetPatientDetailResponse>(patient);

        result.Name.Should().Be(patient.Name);
        result.Phone.Should().Be(patient.Phone.Value);
        result.Email.Should().Be(patient.Email.Value);
    }
}

[TestClass]
public class DentistProfileTests
{
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Setup() =>
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<DentistProfile>(), NullLoggerFactory.Instance).CreateMapper();

    [TestMethod]
    public void Configuration_IsValid() =>
        new MapperConfiguration(cfg => cfg.AddProfile<DentistProfile>(), NullLoggerFactory.Instance).AssertConfigurationIsValid();

    [TestMethod]
    public void Map_MapsValueObjectValues()
    {
        var dentist = TestData.NewDentist();

        var result = _mapper.Map<CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail.GetDentistDetailResponse>(dentist);

        result.Name.Should().Be(dentist.Name);
        result.Phone.Should().Be(dentist.Phone.Value);
        result.Email.Should().Be(dentist.Email.Value);
    }
}

[TestClass]
public class DentalOfficeProfileTests
{
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Setup() =>
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<DentalOfficeProfile>(), NullLoggerFactory.Instance).CreateMapper();

    [TestMethod]
    public void Configuration_IsValid() =>
        new MapperConfiguration(cfg => cfg.AddProfile<DentalOfficeProfile>(), NullLoggerFactory.Instance).AssertConfigurationIsValid();

    [TestMethod]
    public void Map_MapsValueObjectValues()
    {
        var office = TestData.NewDentalOffice();

        var result = _mapper.Map<CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail.GetDentalOfficeDetailResponse>(office);

        result.Name.Should().Be(office.Name);
        result.Phone.Should().Be(office.Phone.Value);
        result.Email.Should().Be(office.Email.Value);
    }
}

[TestClass]
public class AppointmentProfileTests
{
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Setup() =>
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AppointmentProfile>(), NullLoggerFactory.Instance).CreateMapper();

    [TestMethod]
    public void Configuration_IsValid() =>
        new MapperConfiguration(cfg => cfg.AddProfile<AppointmentProfile>(), NullLoggerFactory.Instance).AssertConfigurationIsValid();

    [TestMethod]
    public void Map_MapsNavigationNamesAndInterval()
    {
        var appointment = TestData.NewAppointmentWithNavigation();

        var result = _mapper.Map<GetAppointmentDetailResponse>(appointment);

        result.PatientName.Should().Be(appointment.Patient!.Name);
        result.DentistName.Should().Be(appointment.Dentist!.Name);
        result.DentalOfficeName.Should().Be(appointment.DentalOffice!.Name);
        result.Status.Should().Be(appointment.Status.ToString());
        result.StartDate.Should().Be(appointment.TimeInterval.Start);
        result.EndDate.Should().Be(appointment.TimeInterval.End);
    }
}

[TestClass]
public class TreatmentProfileTests
{
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Setup() =>
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TreatmentProfile>(), NullLoggerFactory.Instance).CreateMapper();

    [TestMethod]
    public void Configuration_IsValid() =>
        new MapperConfiguration(cfg => cfg.AddProfile<TreatmentProfile>(), NullLoggerFactory.Instance).AssertConfigurationIsValid();

    [TestMethod]
    public void Map_MapsNamesThroughAppointmentNavigation()
    {
        var appointment = TestData.NewAppointmentWithNavigation();
        var treatment = TestData.NewTreatment(appointment.Id);
        TestData.SetPrivate(treatment, "Appointment", appointment);

        var result = _mapper.Map<GetTreatmentDetailResponse>(treatment);

        result.PatientName.Should().Be(appointment.Patient!.Name);
        result.DentistName.Should().Be(appointment.Dentist!.Name);
        result.DentalOfficeName.Should().Be(appointment.DentalOffice!.Name);
    }
}
