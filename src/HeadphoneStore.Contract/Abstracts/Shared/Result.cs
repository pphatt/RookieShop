using System.Text.Json.Serialization;

namespace HeadphoneStore.Contract.Abstracts.Shared;

public class Result
{
    protected internal Result(bool isSuccessful)
    {
        IsSuccessful = isSuccessful;
    }

    protected internal Result(bool isSuccessful, string message)
    {
        IsSuccessful = isSuccessful;
        Message = message;
    }

    protected internal Result(bool isSuccessful, List<string> messages)
    {
        IsSuccessful = isSuccessful;
        Messages = messages;
    }

    private List<string> _messages = new();

    public List<string>? Messages
    {
        get => _messages.Any() ? _messages : null;
        set => _messages = value ?? new();
    }

    private string _message = string.Empty;

    public string? Message
    {
        get => _message.Any() ? _message : null;
        set => _message = value ?? string.Empty;
    }

    public bool IsSuccessful { get; }

    public static Result Success() => new(true);

    public static Result Success(string message) => new(true, message);

    public static Result Success(List<string> messages) => new(true, messages);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true);

    public static Result<TValue> Create<TValue>(TValue value) => Success(value);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess) : base(isSuccess)
    {
        _value = value;
    }

    public TValue Value => IsSuccessful ? _value! : throw new InvalidOperationException("The value of failure result can not be access");

    [JsonConstructor]
    public Result(TValue value) : this(value, true) { }

    public static implicit operator Result<TValue>(TValue? value) => Create(value)!;
}
