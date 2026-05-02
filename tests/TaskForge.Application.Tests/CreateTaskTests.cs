using Moq;
using TaskForge.Application.Commands.Tasks.CreateTask;
using TaskForge.Domain.Entities;
using FluentAssertions;
using TaskForge.Application.Common.Interfaces;

namespace TaskForge.Application.Tests;

public class CreateTaskTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;

    internal CreateTaskTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _eventPublisherMock = new Mock<IEventPublisher>();
    }

    [Fact]
    public async Task Handle_Should_CreateTask_WithCorrectTitle()
    {
        // Arrange

        var handler = new CreateTaskHandler(_taskRepositoryMock.Object, _eventPublisherMock.Object);

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
            
        var handler = new CreateTaskHandler(_taskRepositoryMock.Object, _eventPublisherMock.Object);

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
