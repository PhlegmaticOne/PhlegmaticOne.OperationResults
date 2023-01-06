namespace PhlegmaticOne.OperationResults;

public static class DomainResultExtensions
{
    public static DomainResult<TValue> Ensure<TValue>(
        this DomainResult<TValue> result,
        Func<TValue, bool> predicate,
        DomainError error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value!) ? result : DomainResult.Failure<TValue>(error);
    }

    public static DomainResult<TOut> Map<TIn, TOut>(
        this DomainResult<TIn> result,
        Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? DomainResult.Success(mappingFunc(result.Value!))
            : DomainResult.Failure<TOut>(result.DomainError);
    }
}