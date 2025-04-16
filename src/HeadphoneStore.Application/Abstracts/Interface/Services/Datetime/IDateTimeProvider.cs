namespace HeadphoneStore.Application.Abstracts.Interface.Services.Datetime;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
