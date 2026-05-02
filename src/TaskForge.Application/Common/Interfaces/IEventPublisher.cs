using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForge.Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string messageType, string payload);
    }
}
