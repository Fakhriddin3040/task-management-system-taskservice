using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.Structs;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskService.Core.Models;
using TaskManagementSystem.TaskService.Core.Services.NumeralRank;

namespace TaskManagementSystem.TaskService.Core.Aggregates;


public class TaskAggregate : TaskModel
{
    public TaskAggregate() {}

    public static async Task<Result<TaskAggregate>> Create(
        string title,
        string description,
        Guid boardId,
        Guid assignedToId,
        Guid columnId,
        Guid createdById,
        long deadLine,
        CancellationToken cancellationToken,
        GetLatestTaskRankService getLatestTaskRankService,
        IDateTimeService dateTimeService,
        IValidTaskTitlePolicy titlePolicy,
        IValidTaskDescriptionPolicy descriptionPolicy)
    {
        var errors = new List<AppExceptionDetail>();

        ValidateTitle(title, titlePolicy, errors);
        ValidateDescription(description, descriptionPolicy, errors);
        ValidateRequiredBoardId(boardId, errors);
        ValidateRequiredColumnId(columnId, errors);

        if (errors.Any())
        {
            return Result<TaskAggregate>.Failure(errors);
        }

        var newRank = await getLatestTaskRankService.GetLatestRankAsync(
            boardId: boardId,
            cancellationToken: cancellationToken
        );

        var result = new TaskAggregate {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            BoardId = boardId,
            AssignedTo = assignedToId,
            ColumnId = columnId,
            Timestamps = Timestamps.FromDateTimeService(dateTimeService),
            AuthorInfo = new AuthorInfo(createdById, createdById),
            Rank = newRank,
            Deadline = deadLine,
        };

        return Result<TaskAggregate>.Success(result);
    }

    public Result<Unit> Update(
            string title,
            string description,
            Guid assignedToId,
            long deadLine,
            Guid updatedById,
            CancellationToken cancellationToken,
            IValidTaskTitlePolicy titlePolicy,
            IValidTaskDescriptionPolicy descriptionPolicy,
            IDateTimeService dateTimeService)
    {
        var errors = new List<AppExceptionDetail>();

        ValidateTitle(title, titlePolicy, errors);
        ValidateDescription(description, descriptionPolicy, errors);

        if (errors.Any())
        {
            return Result<Unit>.Failure(errors);
        }

        Title = title;
        Description = description;
        AssignedTo = assignedToId;
        Deadline = deadLine;
        Touch(dateTimeService, updatedById);

        return Result<Unit>.Success(Unit.Value);
    }

    public async Task MoveAsync(
        Guid columnId,
        Guid updatedById,
        NumeralRankGenerationService numeralRankGenerationService,
        NumeralRankContext rankContext,
        CancellationToken cancellationToken,
        IDateTimeService dateTimeService
    )
    {
        var newRank = await numeralRankGenerationService.GetNewRankAsync(
            boardId: BoardId,
            context: rankContext,
            cancellationToken: cancellationToken);

        Rank = newRank;
        ColumnId = columnId.IsEmpty() ? ColumnId : columnId;
        Touch(dateTimeService, updatedById);
    }

    public void Delete() {}

    private static void ValidateTitle(string title, IValidTaskTitlePolicy policy, List<AppExceptionDetail> errors)
    {
        if (!policy.IsValid(title))
        {
            errors.Add(
                new AppExceptionDetail(
                    StatusCode: AppExceptionStatusCode.InvalidValue,
                    Field: TaskField.Name
                )
            );
        }
    }

    private static void ValidateDescription(string description, IValidTaskDescriptionPolicy policy, List<AppExceptionDetail> errors)
    {
        if (!policy.IsValid(description))
        {
            errors.Add(
                new AppExceptionDetail(
                    StatusCode: AppExceptionStatusCode.InvalidValue,
                    Field: TaskField.Description
                )
            );
        }
    }

    private static void ValidateRequiredBoardId(Guid boardId, List<AppExceptionDetail> errors)
    {
        if (boardId.IsEmpty())
        {
            errors.Add(
                new AppExceptionDetail(
                    StatusCode: AppExceptionStatusCode.RequiredField,
                    Field: TaskField.BoardId
                )
            );
        }
    }

    private static void ValidateRequiredColumnId(Guid columnId, List<AppExceptionDetail> errors)
    {
        if (columnId.IsEmpty())
        {
            errors.Add(
                new AppExceptionDetail(
                    StatusCode: AppExceptionStatusCode.RequiredField,
                    Field: TaskField.ColumnId
                )
            );
        }
    }

    private void Touch(IDateTimeService dateTimeService, Guid updatedById)
    {
        Timestamps.Touch(dateTimeService);
        AuthorInfo.Update(updatedById);
    }
}
