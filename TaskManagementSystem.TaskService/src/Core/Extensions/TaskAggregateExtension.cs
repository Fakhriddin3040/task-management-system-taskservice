using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;

namespace TaskManagementSystem.TaskService.Core.Extensions;


public static class TaskAggregateExtension
{
    public static AppException TaskNotFoundException()
    {
        return new AppException(
            statusCode: AppExceptionStatusCode.NotFound,
            message: AppExceptionErrorMessages.NotFound
        );
    }
}
