using Moq;
using VAlgo.BuildingBlocks.Sandbox.Abstractions;
using FluentAssertions;
using VAlgo.BuildingBlocks.Sandbox.Models;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Commands.RunCode;
using VAlgo.SharedKernel.CrossModule.Submissions;

public class RunCodeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCode_ReturnsSuccess()
    {
        // Arrange
        var mockService = new Mock<IRunCodeService>();

        var problemId = Guid.NewGuid();

        var command = new RunCodeCommand(
            problemId,
            "python",
            "print('Hello')"
        );

        var expected = new RunCodeResultDto
        {
            Verdict = Verdict.Accepted.ToString()
        };

        mockService
            .Setup(x => x.RunAsync(
                problemId,
                command.Language,
                command.SourceCode,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expected);

        var handler = new RunCodeCommandHandler(mockService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);

        mockService.Verify(x => x.RunAsync(
            problemId,
            command.Language,
            command.SourceCode,
            It.IsAny<CancellationToken>()
        ));

        
    }
}