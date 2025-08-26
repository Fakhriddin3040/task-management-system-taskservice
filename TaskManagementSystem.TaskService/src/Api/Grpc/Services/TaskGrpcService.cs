using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using TaskManagementSystem.GrpcLib.TaskService.Types;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.TaskService.Application.Commands.Commands;
using TaskManagementSystem.TaskService.Application.Queries.Queries;

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
        _logger.LogInformation("Received request to create task with title: {Title}", request.Title);
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

    public override async Task<Empty> UpdateTask(TaskUpdateProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Received request to update task with ID: {Id}", request.Id);
        var command = new TaskUpdateCommand(
            id: request.Id.ToGuidOrDefault(),
            title: request.Title,
            description: request.Description,
            deadline: request.Deadline,
            assignedToId: request.AssignedToId.ToGuidOrDefault()
        );

        _logger.LogInformation("Updating task with ID: {Id}", request.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update task with ID: {Id}", request.Id);
            throw result.CreateExceptionFrom();
        }

        return new Empty();
    }

    public override async Task<Empty> MoveTask(TaskMoveProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Received request to move task with ID: {Id}", request.Id);

        var command = new TaskMoveCommand(
            id: request.Id.ToGuidOrDefault(),
            columnId: request.ColumnId.ToGuidOrDefault(),
            previousTaskId: request.PreviousTaskId.ToGuidOrDefault(),
            nextTaskId: request.NextTaskId.ToGuidOrDefault()
        );

        await _mediator.Send(command, context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> DeleteTask(TaskDeleteProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Received request to delete task with ID: {Id}", request.Id);

        var command = new TaskDeleteCommand(
            id: request.Id.ToGuidOrDefault()
        );

        await _mediator.Send(command, context.CancellationToken);
        return new Empty();
    }

    public override async Task<TaskGetByBoardIdProtoResponse> GetByBoardId(TaskGetByBoardIdProtoRequest request, ServerCallContext context)
    {
        var query = new GetByBoardIdQuery(boardId: request.BoardId.ToGuidOrDefault());

        var result = await _mediator.Send(query, context.CancellationToken);
        var response = new TaskGetByBoardIdProtoResponse();

        IEnumerable<TaskGetByBoardIdProtoMessage> responseTasks = result.Tasks.Select(dto => new TaskGetByBoardIdProtoMessage {
            Id = dto.Id.ToString(),
            Title = dto.Title,
            AssignedToId = dto.AssignedToId.ToString(),
            ColumnId = dto.ColumnId.ToString(),
            Rank = dto.Rank
        });

        response.Tasks.AddRange(responseTasks);

        return response;
    }

    public override async Task<TaskGetByColumnIdProtoResponse> GetByColumnId(TaskGetByColumnIdProtoRequest request, ServerCallContext context)
    {
        var command = new GetByColumnIdQuery(request.ColumnId.ToGuidOrDefault());

        var result = await _mediator.Send(command, context.CancellationToken);

        var response = new TaskGetByColumnIdProtoResponse();

        var responseTasks = result.Tasks.Select(dto => new TaskGetByColumnIdProtoMessage {
            Id = dto.Id.ToString(),
            Title = dto.Title,
            AssignedToId = dto.AssignedToId.ToString(),
            Rank = dto.Rank
        });
        response.Tasks.AddRange(responseTasks);

        return response;
    }

    public override async Task<TaskGetDetailedProtoResponse> GetById(TaskGetDetailedProtoRequest request, ServerCallContext context)
    {
        var command = new GetDetailedQuery(id: request.Id.ToGuidOrDefault());
        var result = await _mediator.Send(command, context.CancellationToken);

        return new TaskGetDetailedProtoResponse {
            Id = result.Id.ToString(),
            Title = result.Title,
            Description = result.Description,
            AssignedToId = result.AssignedToId.ToString(),
            Deadline = result.Deadline,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            CreatedById = result.CreatedById.ToString(),
            UpdatedById = result.UpdatedById.ToString(),
            ColumnId = result.ColumnId.ToString(),
            BoardId = result.BoardId.ToString(),
            Rank = result.Rank
        };
    }
}
