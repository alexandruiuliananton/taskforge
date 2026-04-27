using Moq;
using TaskForge.Application.Commands.Tasks.CreateTask;
using TaskForge.Application.Interfaces;
using TaskForge.Domain.Entities;
using FluentAssertions;

namespace TaskForge.Application.Tests;

public class CreateTaskTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;

    internal CreateTaskTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateTask_WithCorrectTitle()
    {
        // Arrange

        var handler = new CreateTaskHandler(_taskRepositoryMock.Object);

        var command = new CreateTaskCommand
        {
            Title = "Test task"
        };

        TaskItem? captured = null;

        _taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(t => captured = t)
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command);

        // Assert
        captured.Should().NotBeNull();
        captured!.Title.Should().Be("Test task");
    }

    [Fact]
    public async Task Handle_Should_CallRepositoryOnce()
    {
        // Arrange

        var handler = new CreateTaskHandler(_taskRepositoryMock.Object);

        var command = new CreateTaskCommand
        {
            Title = "Test task"
        };

        _taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command);

        // Assert
        _taskRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }
}
