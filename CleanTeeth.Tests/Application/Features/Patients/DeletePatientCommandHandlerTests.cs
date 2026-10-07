using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Patients.Commands.DeletePatient;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Patients;

[TestClass]
public class DeletePatientCommandHandlerTests
{
    private IPatientRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IUserService _userService = null!;
    private DeletePatientCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IPatientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userService = Substitute.For<IUserService>();
        _userService.UserId.Returns(Guid.NewGuid());
        _handler = new DeletePatientCommandHandler(_repository, _unitOfWork, _userService);
    }

    [TestMethod]
    public async Task Handle_PatientExists_SoftDeletesAndCommits()
    {
        // Arrange
        var patient = TestData.NewPatient();
        var command = new DeletePatientCommand { Id = patient.Id };
        _repository.GetById(patient.Id, Arg.Any<CancellationToken>()).Returns(patient);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        patient.IsDeleted.Should().BeTrue();
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_PatientNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeletePatientCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Patient?)null);

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
        var patient = TestData.NewPatient();
        var command = new DeletePatientCommand { Id = patient.Id };
        _repository.GetById(patient.Id, Arg.Any<CancellationToken>()).Returns(patient);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
