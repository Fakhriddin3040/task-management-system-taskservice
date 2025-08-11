using Grpc.Core;
using MediatR;
using TaskManagementSystem.GrpcLib.TaskService.Types;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.TaskService.Application.Commands.Commands;

namespace TaskManagementSystem.TaskService.Api.Grpc.Services;


public class TaskGrpcService : GrpcLib.TaskService.Services.TaskService.TaskServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskGrpcService> _logger;

    public TaskGrpcService(
        IMediator mediator,
        ILogger<TaskGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<TaskCreateProtoResponse> CreateTask(TaskCreateProtoRequest request, ServerCallContext context)
    {
        var command = new TaskCreateCommand(
            boardId: request.BoardId.ToGuidOrDefault(),
            columnId: request.ColumnId.ToGuidOrDefault(),
            assignedToId: request.AssignedToId.ToGuidOrDefault(),
            title: request.Title,
            description: request.Description,
            deadline: request.Deadline
        );

        _logger.LogInformation("Creating task with title: {Title}", request.Title);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new TaskCreateProtoResponse {
            Id = result.GetValueOrThrow().Id.ToString()
        };
    }
}
