using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.Appointments;

[TestClass]
public class GetAppointmentListQueryHandlerTests
{
    private IAppointmentRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetAppointmentListQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IAppointmentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAppointmentListQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsPagedMappedResult()
    {
        // Arrange
        var appointment = TestData.NewAppointmentWithNavigation();
        var paged = new PagedResult<Appointment>(new List<Appointment> { appointment }, 1, 1, 10);
        _repository.GetPagedAsync(
            Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<GetAppointmentListQuery>(), Arg.Any<CancellationToken>()).Returns(paged);
        var mapped = new List<GetAppointmentListResponse>
        {
            new()
            {
                Id = appointment.Id,
                PatientName = "Patient",
                DentistName = "Dentist",
                DentalOfficeName = "Office",
                Status = "Scheduled",
                StartDate = appointment.TimeInterval.Start,
                EndDate = appointment.TimeInterval.End
            }
        };
        _mapper.Map<List<GetAppointmentListResponse>>(Arg.Any<object>()).Returns(mapped);

        // Act
        var result = await _handler.Handle(
            new GetAppointmentListQuery { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }
}

[TestClass]
public class GetAppointmentDetailQueryHandlerTests
{
    private IAppointmentRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetAppointmentDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IAppointmentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAppointmentDetailQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_AppointmentExists_ReturnsMappedResponse()
    {
        // Arrange
        var appointment = TestData.NewAppointmentWithNavigation();
        var expected = new GetAppointmentDetailResponse
        {
            Id = appointment.Id,
            PatientName = "Patient",
            DentistName = "Dentist",
            DentalOfficeName = "Office",
            Status = "Scheduled",
            StartDate = appointment.TimeInterval.Start,
            EndDate = appointment.TimeInterval.End
        };
        _repository.GetById(appointment.Id, Arg.Any<CancellationToken>()).Returns(appointment);
        _mapper.Map<GetAppointmentDetailResponse>(appointment).Returns(expected);

        // Act
        var result = await _handler.Handle(
            new GetAppointmentDetailQuery { Id = appointment.Id }, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [TestMethod]
    public async Task Handle_AppointmentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns((Appointment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(
            new GetAppointmentDetailQuery { Id = id }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
