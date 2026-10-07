using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class DeleteDentistCommandHandlerTests
{
    private IDentistRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IUserService _userService = null!;
    private DeleteDentistCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userService = Substitute.For<IUserService>();
        _userService.UserId.Returns(Guid.NewGuid());
        _handler = new DeleteDentistCommandHandler(_repository, _unitOfWork, _userService);
    }

    [TestMethod]
    public async Task Handle_DentistExists_SoftDeletesAndCommits()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var command = new DeleteDentistCommand { Id = dentist.Id };
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        dentist.IsDeleted.Should().BeTrue();
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_DentistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteDentistCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Dentist?)null);

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
        var dentist = TestData.NewDentist();
        var command = new DeleteDentistCommand { Id = dentist.Id };
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
