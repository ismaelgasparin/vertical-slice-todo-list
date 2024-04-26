using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceTodoList.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result,
        Func<Result<T>, IActionResult>? onSuccess = default,
        Func<Result<T>, IActionResult>? onFailed = default)
    {
        IActionResult onSuccessFunc() =>
            onSuccess is null
                ? new OkObjectResult(result.Value)
                : onSuccess(result);

        IActionResult onFailedFunc() =>
            onFailed is null
                ? new BadRequestObjectResult(result.Errors)
                : onFailed(result);

        return result.IsSuccess
            ? onSuccessFunc()
            : onFailedFunc();
    }

    public static IActionResult ToActionResult(this Result result,
        Func<Result, IActionResult>? onSuccess = default,
        Func<Result, IActionResult>? onFailed = default)
    {
        IActionResult onSuccessFunc() =>
            onSuccess is null
                ? new OkResult()
                : onSuccess(result);

        IActionResult onFailedFunc() =>
            onFailed is null
                ? new BadRequestObjectResult(result.Errors)
                : onFailed(result);

        return result.IsSuccess
            ? onSuccessFunc()
            : onFailedFunc();
    }

    public static IActionResult ToActionResult<T>(this Result<List<T>> result)
    {
        static IActionResult onSuccessFunc(Result<List<T>> result) =>
            result.Value.Count > 0
                ? new OkObjectResult(result.Value)
                : new NoContentResult();

        return result.ToActionResult(
            onSuccess: onSuccessFunc);
    }
}
