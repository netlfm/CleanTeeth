using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Dentists.Commands.InactiveDentist;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class InactiveDentistCommandHandlerTests
{
    private IDentistRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private InactiveDentistCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new InactiveDentistCommandHandler(_repository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_DentistExists_SetsInactiveAndCommits()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var command = new InactiveDentistCommand { Id = dentist.Id };
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        dentist.Status.Should().Be(DentistStatus.Inactive);
        await _repository.Received(1).Update(dentist, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_DentistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new InactiveDentistCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Dentist?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task Handle_UpdateThrows_RollbackIsCalled()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var command = new InactiveDentistCommand { Id = dentist.Id };
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);
        _repository.Update(Arg.Any<Dentist>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
