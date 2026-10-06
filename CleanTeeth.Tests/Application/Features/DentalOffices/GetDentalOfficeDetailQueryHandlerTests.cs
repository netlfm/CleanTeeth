using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class GetDentalOfficeDetailQueryHandlerTests
{
    private IDentalOfficeRepository _repository = null!;
    private IMapper _mapper = null!;
    private GetDentalOfficeDetailQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentalOfficeRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetDentalOfficeDetailQueryHandler(_repository, _mapper);
    }

    [TestMethod]
    public async Task Handle_DentalOfficeExists_ReturnsMappedResponse()
    {
        // Arrange
        var dentalOffice = new DentalOffice("Dental Office 1", "Hauptstr. 1", new PhoneNumber("+4912345678"), new Email("office@test.com"));
        var id = dentalOffice.Id;
        var expected = new GetDentalOfficeDetailResponse { Id = id, Name = "Dental Office 1" };
        var query = new GetDentalOfficeDetailQuery { Id = id };

        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns(dentalOffice);
        _mapper.Map<GetDentalOfficeDetailResponse>(dentalOffice).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [TestMethod]
    public async Task Handle_DentalOfficeDoesNotExist_ThrowsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetDentalOfficeDetailQuery { Id = id };

        _repository.GetById(id, Arg.Any<CancellationToken>()).Returns((DentalOffice?)null);

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
