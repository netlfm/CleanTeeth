using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.Treatments;

[TestClass]
public class GetTreatmentListQueryHandlerTests
{
    private ITreatmentRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetTreatmentListQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<ITreatmentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetTreatmentListQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsPagedMappedResult()
    {
        // Arrange
        var treatment = TestData.NewTreatment();
        var paged = new PagedResult<Treatment>(new List<Treatment> { treatment }, 1, 1, 10);
        _repository.GetPagedAsync(
            Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<GetTreatmentListQuery>(), Arg.Any<CancellationToken>()).Returns(paged);
        var mapped = new List<GetTreatmentListResponse>
        {
            new() { Id = treatment.Id, Status = TreatmentStatus.InProgress }
        };
        _mapper.Map<List<GetTreatmentListResponse>>(Arg.Any<object>()).Returns(mapped);

        // Act
        var result = await _handler.Handle(
            new GetTreatmentListQuery { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }
}

[TestClass]
public class GetTreatmentDetailQueryHandlerTests
{
    private ITreatmentRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetTreatmentDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<ITreatmentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetTreatmentDetailQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_TreatmentExists_ReturnsMappedResponse()
    {
        // Arrange
        var treatment = TestData.NewTreatment();
        var expected = new GetTreatmentDetailResponse
        {
            Id = treatment.Id,
            PatientName = "Patient",
            DentistName = "Dentist",
            DentalOfficeName = "Office"
        };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);
        _mapper.Map<GetTreatmentDetailResponse>(treatment).Returns(expected);

        // Act
        var result = await _handler.Handle(
            new GetTreatmentDetailQuery { Id = treatment.Id }, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [TestMethod]
    public async Task Handle_TreatmentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns((Treatment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(
            new GetTreatmentDetailQuery { Id = id }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
