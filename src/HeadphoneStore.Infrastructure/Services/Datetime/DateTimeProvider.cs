using HeadphoneStore.Application.Abstracts.Interface.Services.Datetime;

namespace HeadphoneStore.Infrastructure.Services.Datetime;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
