using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Dentists.Commands.CreateDentist;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class CreateDentistCommandHandlerTests
{
    private IDentistRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CreateDentistCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateDentistCommandHandler(_repository, _unitOfWork);
    }

    private static CreateDentistCommand CreateValidCommand() => new()
    {
        UserId = Guid.NewGuid(),
        Name = "Dr. Smith",
        Gender = Gender.Male,
        Phone = "+4912345678",
        Email = "smith@example.com",
        LicenseNumber = "LIC-123",
        Specialty = "Orthodontics"
    };

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsDentistId()
    {
        // Arrange
        var command = CreateValidCommand();
        var dentist = new Dentist(
            command.UserId, command.Name, command.Gender,
            new PhoneNumber(command.Phone), new Email(command.Email),
            command.LicenseNumber, command.Specialty);
        _repository.Add(Arg.Any<Dentist>(), Arg.Any<CancellationToken>()).Returns(dentist);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(dentist.Id);
        await _repository.Received(1).Add(Arg.Any<Dentist>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var command = CreateValidCommand();
        _repository.Add(Arg.Any<Dentist>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
