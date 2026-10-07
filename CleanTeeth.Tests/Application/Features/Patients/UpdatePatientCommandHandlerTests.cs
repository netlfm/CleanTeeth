using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Patients;

[TestClass]
public class UpdatePatientCommandHandlerTests
{
    private IPatientRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private UpdatePatientCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IPatientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdatePatientCommandHandler(_repository, _unitOfWork);
    }

    private static UpdatePatientCommand CreateCommand(Guid id) => new()
    {
        Id = id,
        Name = "Updated Name",
        DateOfBirth = new DateOnly(1995, 5, 5),
        Gender = Gender.Female,
        Phone = "+4998765432",
        Email = "updated@test.com",
        Address = "Updated Address"
    };

    [TestMethod]
    public async Task Handle_PatientExists_UpdatesAndCommits()
    {
        // Arrange
        var patient = TestData.NewPatient();
        var command = CreateCommand(patient.Id);
        _repository.GetById(patient.Id, Arg.Any<CancellationToken>()).Returns(patient);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        patient.Name.Should().Be("Updated Name");
        patient.Gender.Should().Be(Gender.Female);
        patient.Email.Value.Should().Be("updated@test.com");
        await _repository.Received(1).Update(patient, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_PatientNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid());
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Patient?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().Update(Arg.Any<Patient>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_UpdateThrows_RollbackIsCalled()
    {
        // Arrange
        var patient = TestData.NewPatient();
        var command = CreateCommand(patient.Id);
        _repository.GetById(patient.Id, Arg.Any<CancellationToken>()).Returns(patient);
        _repository.Update(Arg.Any<Patient>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
