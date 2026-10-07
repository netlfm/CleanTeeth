using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DeleteDentalOfficeCommandHandlerTests
{
    private IDentalOfficeRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IUserService _userService = null!;
    private DeleteDentalOfficeCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentalOfficeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userService = Substitute.For<IUserService>();
        _userService.UserId.Returns(Guid.NewGuid());
        _handler = new DeleteDentalOfficeCommandHandler(_repository, _unitOfWork, _userService);
    }

    [TestMethod]
    public async Task Handle_OfficeExists_SoftDeletesAndCommits()
    {
        // Arrange
        var office = TestData.NewDentalOffice();
        var command = new DeleteDentalOfficeCommand { Id = office.Id };
        _repository.GetById(office.Id, Arg.Any<CancellationToken>()).Returns(office);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        office.IsDeleted.Should().BeTrue();
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_OfficeNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteDentalOfficeCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DentalOffice?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _unitOfWork.DidNotReceive().Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_CommitThrows_RollbackIsCalled()
    {
        // Arrange
        var office = TestData.NewDentalOffice();
        var command = new DeleteDentalOfficeCommand { Id = office.Id };
        _repository.GetById(office.Id, Arg.Any<CancellationToken>()).Returns(office);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
