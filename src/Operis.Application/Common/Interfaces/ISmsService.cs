using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Operis.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string to, string message);
    }
}
