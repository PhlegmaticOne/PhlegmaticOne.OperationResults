namespace PhlegmaticOne.OperationResults;

public class DomainError : IEquatable<DomainError>
{
    public static DomainError None => new(string.Empty, string.Empty);
    public static DomainError NullValue => new("DomainError.NullValue", "Specified result value is null");
    public DomainError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(DomainError error) => error.Code;

    public static bool operator ==(DomainError? a, DomainError? b)
    {
        if (a is null && b is null)
        {
            return false;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Code == b.Code;
    }


    public static bool operator !=(DomainError? a, DomainError? b) => !(a == b);
    public bool Equals(DomainError? other) => this == other;
    public override bool Equals(object? obj) => this == obj as DomainError;
    public override int GetHashCode() => Code.GetHashCode();
}