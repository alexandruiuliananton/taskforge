using System;
using System.Collections.Generic;
using System.Text;

namespace Operis.Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string messageType, string payload);
    }
}
