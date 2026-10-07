using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class GetDentalOfficeListQueryHandlerTests
{
    private IDentalOfficeRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetDentalOfficeListQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentalOfficeRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetDentalOfficeListQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsOfficesForCurrentUser()
    {
        // Arrange
        var office = TestData.NewDentalOffice();
        _repository.GetForCurrentUser(Arg.Any<CancellationToken>())
            .Returns(new List<DentalOffice> { office });
        var mapped = new List<GetDentalOfficeListResponse>
        {
            new() { Id = office.Id, Name = "Main Office" }
        };
        _mapper.Map<List<GetDentalOfficeListResponse>>(Arg.Any<object>()).Returns(mapped);

        // Act
        var result = await _handler.Handle(new GetDentalOfficeListQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Main Office");
    }
}
