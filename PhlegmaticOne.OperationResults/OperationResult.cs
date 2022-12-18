namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static OperationResult Success => new()
    {
        IsSuccess = true
    };

    public static OperationResult<T> Successful<T>(T result)
    {
        return new()
        {
            IsSuccess = true,
            ErrorMessage = null,
            Result = result
        };
    }

    public static OperationResult<T> Failed<T>(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error",
            Result = default
        };
    }

    public static OperationResult Failed(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error"
        };
    }

    public static async Task<OperationResult<T>> FromActionResult<T>(Func<Task<T>> operation)
    {
        try
        {
            var result = await operation();
            return Successful(result);
        }
        catch (Exception ex)
        {
            return Failed<T>(ex.Message);
        }
    }

    public static async Task<OperationResult> FromActionResult(Func<Task> operation)
    {
        try
        {
            await operation();
            return Success;
        }
        catch (Exception ex)
        {
            return Failed(ex.Message);
        }
    }
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }
}