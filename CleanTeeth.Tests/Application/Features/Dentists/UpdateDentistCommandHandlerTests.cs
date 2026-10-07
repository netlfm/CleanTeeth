using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class UpdateDentistCommandHandlerTests
{
    private IDentistRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private UpdateDentistCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<IDentistRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateDentistCommandHandler(_repository, _unitOfWork);
    }

    private static UpdateDentistCommand CreateCommand(Guid id) => new()
    {
        Id = id,
        Name = "Updated Dentist",
        Gender = Gender.Female,
        Phone = "+4998765432",
        Email = "updated@test.com",
        LicenseNumber = "LIC-999",
        Specialty = "Endodontics"
    };

    [TestMethod]
    public async Task Handle_DentistExists_UpdatesAndCommits()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var command = CreateCommand(dentist.Id);
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        dentist.Name.Should().Be("Updated Dentist");
        dentist.LicenseNumber.Should().Be("LIC-999");
        dentist.Specialty.Should().Be("Endodontics");
        await _repository.Received(1).Update(dentist, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_DentistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid());
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Dentist?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().Update(Arg.Any<Dentist>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_UpdateThrows_RollbackIsCalled()
    {
        // Arrange
        var dentist = TestData.NewDentist();
        var command = CreateCommand(dentist.Id);
        _repository.GetById(dentist.Id, Arg.Any<CancellationToken>()).Returns(dentist);
        _repository.Update(Arg.Any<Dentist>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
