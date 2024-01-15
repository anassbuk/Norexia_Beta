using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
