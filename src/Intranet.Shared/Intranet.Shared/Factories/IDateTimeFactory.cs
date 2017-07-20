using System;

namespace Intranet.Shared.Factories
{
    public interface IDateTimeFactory
    {
        DateTime DateTime { get; }
        DateTimeOffset DateTimeOffset { get; }
        DateTimeOffset DateTimeOffsetUtc { get; }
    }
}