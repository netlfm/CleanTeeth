using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.Patients;

[TestClass]
public class GetPatientListQueryHandlerTests
{
    private IPatientRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetPatientListQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IPatientRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetPatientListQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsPagedMappedResult()
    {
        // Arrange
        var patient = TestData.NewPatient();
        var paged = new PagedResult<Patient>(new List<Patient> { patient }, 1, 1, 10);
        _repository.GetPagedAsync(
            Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<GetPatientListQuery>(), Arg.Any<CancellationToken>()).Returns(paged);
        var mappedItems = new List<GetPatientListResponse>
        {
            new() { Id = patient.Id, Name = "John Doe" }
        };
        _mapper.Map<List<GetPatientListResponse>>(Arg.Any<object>()).Returns(mappedItems);

        // Act
        var result = await _handler.Handle(
            new GetPatientListQuery { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}

[TestClass]
public class GetPatientDetailQueryHandlerTests
{
    private IPatientRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetPatientDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IPatientRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetPatientDetailQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_PatientExists_ReturnsMappedResponse()
    {
        // Arrange
        var patient = TestData.NewPatient();
        var expected = new GetPatientDetailResponse { Id = patient.Id, Name = "John" };
        _repository.GetById(patient.Id, Arg.Any<CancellationToken>()).Returns(patient);
        _mapper.Map<GetPatientDetailResponse>(patient).Returns(expected);

        // Act
        var result = await _handler.Handle(
            new GetPatientDetailQuery { Id = patient.Id }, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [TestMethod]
    public async Task Handle_PatientNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns((Patient?)null);

        // Act
        Func<Task> act = () => _handler.Handle(
            new GetPatientDetailQuery { Id = id }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
