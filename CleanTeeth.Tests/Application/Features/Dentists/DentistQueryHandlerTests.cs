using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class GetDentistListQueryHandlerTests
{
    private IDentistRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetDentistListQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetDentistListQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsPagedMappedResult()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var paged = new PagedResult<Dentist>(new List<Dentist> { dentist }, 1, 1, 10);
        _repository.GetPagedAsync(
            Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<GetDentistListQuery>(), Arg.Any<CancellationToken>()).Returns(paged);
        var mapped = new List<GetDentistListResponse>
        {
            new() { Id = dentist.Id, Name = "Dr. Smith" }
        };
        _mapper.Map<List<GetDentistListResponse>>(Arg.Any<object>()).Returns(mapped);

        // Act
        var result = await _handler.Handle(
            new GetDentistListQuery { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }
}

[TestClass]
public class GetDentistDetailQueryHandlerTests
{
    private IDentistRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetDentistDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetDentistDetailQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_DentistExists_ReturnsMappedResponse()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var expected = new GetDentistDetailResponse { Id = dentist.Id, Name = "Dr. Smith" };
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);
        _mapper.Map<GetDentistDetailResponse>(dentist).Returns(expected);

        // Act
        var result = await _handler.Handle(
            new GetDentistDetailQuery { Id = dentist.Id }, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [TestMethod]
    public async Task Handle_DentistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns((Dentist?)null);

        // Act
        Func<Task> act = () => _handler.Handle(
            new GetDentistDetailQuery { Id = id }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
