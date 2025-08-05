using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskService.Core.Models;
using TaskManagementSystem.TaskService.Core.Services;
using NumeralRankContext = TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskService.Core.Aggregates;


public class TaskAggregate : TaskModel
{
    public TaskAggregate() {}

    public static async Task<Result<TaskAggregate>> Create(
        string name,
        string description,
        Guid boardId,
        Guid? assignedTo,
        Guid columnId,
        Guid createdBy,
        long deadLine,
        long latestRank,
        CancellationToken cancellationToken,
        GetLatestTaskRankService getLatestTaskRankService,
        IDateTimeService dateTimeService,
        IValidTaskNamePolicy namePolicy,
        IValidTaskDescriptionPolicy descriptionPolicy)
    {
        var errors = new List<AppExceptionDetail>();

        ValidateName(name, namePolicy, errors);
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
            Name = name,
            Description = description,
            BoardId = boardId,
            AssignedTo = assignedTo,
            ColumnId = columnId,
            Timestamps = Timestamps.FromDateTimeService(dateTimeService),
            AuthorInfo = new AuthorInfo(createdBy, createdBy),
            Rank = newRank
        };

        return Result<TaskAggregate>.Success(result);
    }

    private static void ValidateName(string name, IValidTaskNamePolicy policy, List<AppExceptionDetail> errors)
    {
        if (!policy.IsValid(name))
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
}
