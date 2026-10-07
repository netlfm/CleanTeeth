using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Patients.Commands.CreatePatient;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Patients;

[TestClass]
public class CreatePatientCommandHandlerTests
{
    private IPatientRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CreatePatientCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IPatientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreatePatientCommandHandler(_repository, _unitOfWork);
    }

    private static CreatePatientCommand CreateValidCommand() => new()
    {
        UserId = Guid.NewGuid(),
        Name = "John Doe",
        DateOfBirth = new DateOnly(1990, 1, 1),
        Gender = Gender.Male,
        Phone = "+4912345678",
        Email = "john@example.com",
        Address = "Some Street 1"
    };

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsPatientId()
    {
        // Arrange
        var command = CreateValidCommand();
        var patient = new Patient(
            command.UserId, command.Name, "P-123",
            command.DateOfBirth, command.Gender,
            new PhoneNumber(command.Phone), new Email(command.Email),
            command.Address);
        _repository.Add(Arg.Any<Patient>(), Arg.Any<CancellationToken>()).Returns(patient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(patient.Id);
        await _repository.Received(1).Add(Arg.Any<Patient>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var command = CreateValidCommand();
        _repository.Add(Arg.Any<Patient>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
