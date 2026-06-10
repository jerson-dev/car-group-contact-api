namespace ContactosApi.Domain;

public enum ResultErrorKind
{
    None,
    Validation,
    Conflict
}

public record Result<T>(T? Value, string? Error, bool IsSuccess, ResultErrorKind ErrorKind = ResultErrorKind.None)
{
    public static Result<T> Success(T value) => new(value, null, true);

    public static Result<T> ValidationFailure(string error) =>
        new(default, error, false, ResultErrorKind.Validation);

    public static Result<T> ConflictFailure(string error) =>
        new(default, error, false, ResultErrorKind.Conflict);
}
