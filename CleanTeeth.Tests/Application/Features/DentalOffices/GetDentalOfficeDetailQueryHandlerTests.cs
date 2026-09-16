using Ardalis.Result;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class GetDentalOfficeDetailQueryHandlerTests
{
    private IDentalOfficeRepository _repository = null!;
    private GetDentalOfficeDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentalOfficeRepository>();
        _handler = new GetDentalOfficeDetailQueryHandler(_repository);
    }
    [TestMethod]
    public async Task Handle_DentalOfficeExists_ReturnsIt()
    {
        // Arrange
        var dentalOffice = new DentalOffice("Dental Office 1");
        var id = dentalOffice.Id;
        var query = new GetDentalOfficeDetailQuery { Id = id };

        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns(dentalOffice);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Id.Should().Be(id);
        result.Value.Name.Should().Be("Dental Office 1");

        // 方式二：对象图结构断言（FluentAssertions 的高阶用法，推荐）
        // result.Should().BeEquivalentTo(new { Id = id, Name = "Dental Office 1" });
    }
    [TestMethod]
    public async Task Handle_DentalOfficeDoesNotExist_Throws()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetDentalOfficeDetailQuery { Id = id };

        _repository.GetById(id, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
    }

}
