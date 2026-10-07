using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Treatments.Command.CancelTreatment;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.Treatments;

[TestClass]
public class CancelTreatmentCommandHandlerTests
{
    private ITreatmentRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CancelTreatmentCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = Substitute.For<ITreatmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CancelTreatmentCommandHandler(_repository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_ExistingTreatment_CancelsAndCommits()
    {
        // Arrange
        var treatment = new Treatment(Guid.NewGuid());
        var command = new CancelTreatmentCommand { Id = treatment.Id };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        treatment.Status.Should().Be(CleanTeeth.Domain.Enums.TreatmentStatus.Cancelled);
        await _repository.Received(1).Update(treatment, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TreatmentNotFound_ThrowsBusinessRuleException()
    {
        // Arrange
        var command = new CancelTreatmentCommand { Id = Guid.NewGuid() };
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Treatment?)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Treatment does not exist.");
        await _repository.DidNotReceive().Update(Arg.Any<Treatment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_RepositoryThrows_RollbackIsCalled()
    {
        // Arrange
        var treatment = new Treatment(Guid.NewGuid());
        var command = new CancelTreatmentCommand { Id = treatment.Id };
        _repository.GetById(treatment.Id, Arg.Any<CancellationToken>()).Returns(treatment);
        _repository.Update(Arg.Any<Treatment>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}
