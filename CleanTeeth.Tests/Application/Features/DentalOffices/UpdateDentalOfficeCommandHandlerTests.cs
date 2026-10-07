using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class UpdateDentalOfficeCommandHandlerTests
{
    private IDentalOfficeRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private UpdateDentalOfficeCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentalOfficeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateDentalOfficeCommandHandler(_unitOfWork, _repository);
    }

    private static UpdateDentalOfficeCommand CreateCommand(Guid id) => new()
    {
        Id = id,
        Name = "Updated Office",
        Address = "Updated Address",
        Phone = "+4998765432",
        Email = "updated@test.com"
    };

    [TestMethod]
    public async Task Handle_OfficeExists_UpdatesAndCommits()
    {
        // Arrange
        var office = TestData.NewDentalOffice();
        var command = CreateCommand(office.Id);
        _repository.GetById(office.Id, Arg.Any<CancellationToken>()).Returns(office);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        office.Name.Should().Be("Updated Office");
        office.Address.Should().Be("Updated Address");
        await _repository.Received(1).Update(office, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_OfficeNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid());
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DentalOffice?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().Update(Arg.Any<DentalOffice>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_UpdateThrows_RollbackIsCalled()
    {
        // Arrange
        var office = TestData.NewDentalOffice();
        var command = CreateCommand(office.Id);
        _repository.GetById(office.Id, Arg.Any<CancellationToken>()).Returns(office);
        _repository.Update(Arg.Any<DentalOffice>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
