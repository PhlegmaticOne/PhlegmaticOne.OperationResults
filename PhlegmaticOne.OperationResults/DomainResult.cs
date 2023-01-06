namespace PhlegmaticOne.OperationResults;

[Serializable]
public class DomainResult
{
    protected DomainResult(bool isSuccess, DomainError domainError)
    {
        DomainError = domainError;
        IsSuccess = isSuccess;
    }

    public DomainError DomainError { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public static DomainResult Success() => new(true, DomainError.None);
    public static DomainResult<TValue> Success<TValue>(TValue value) => new(value, true, DomainError.None);
    public static DomainResult Failure(DomainError error) => new(false, error);
    public static DomainResult<TValue> Failure<TValue>(DomainError error) => new(default, false, error);
    public static DomainResult<TValue> Create<TValue>(TValue? value) =>
        value is null ? Failure<TValue>(DomainError.NullValue) : Success(value);

    public static async Task<DomainResult<T>> FromActionResult<T>(Func<Task<T>> operation)
    {
        try
        {
            var result = await operation();
            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure<T>(new("Exception.Error", ex.Message));
        }
    }

    public static async Task<DomainResult> FromActionResult(Func<Task> operation)
    {
        try
        {
            await operation();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(new("Exception.Error", ex.Message));
        }
    }
}

[Serializable]
public class DomainResult<TValue> : DomainResult
{
    private readonly TValue? _value;
    internal DomainResult(TValue? value, bool isSuccess, DomainError domainError) :
        base(isSuccess, domainError) => _value = value;

    public TValue? Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Result does not have a value since it is has en DomainError");
    public static implicit operator DomainResult<TValue>(TValue? value) => Create(value);
}