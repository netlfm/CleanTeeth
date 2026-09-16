using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class CreateDentalOfficeCommandHandlerTests
{
    // 使用 null! 抑制编译器的非空警告，替代繁琐的 #pragma warning
    private IDentalOfficeRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CreateDentalOfficeCommandHandler handler = null!;

    [TestInitialize]
    public void Setup()
    {
        // 1. Mock 外部依赖
        _repository = Substitute.For<IDentalOfficeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        // 2. 核心修正：必须实例化真实的 Handler，而不是 Mock 它
        handler = new CreateDentalOfficeCommandHandler(_repository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsDentalOfficeId()
    {
        // Arrange
        var command = new CreateDentalOfficeCommand { Name = "Test Dental Office" };
        var dentalOffice = new DentalOffice("Test Dental Office");

        // 3. 配置 Mock 行为
        _repository.Add(Arg.Any<DentalOffice>(), Arg.Any<CancellationToken>()).Returns(dentalOffice);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(dentalOffice.Id);

        await _repository.Received(1).Add(Arg.Any<DentalOffice>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
    }
    [TestMethod]
    public async Task Handle_WhenTheresAnError_WeRollback()
    {
        // Arrange
        var command = new CreateDentalOfficeCommand { Name = "Test Dental Office" };
        _repository.Add(Arg.Any<DentalOffice>(), Arg.Any<CancellationToken>()).Throws<Exception>();

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).Rollback(Arg.Any<CancellationToken>());
    }
}