using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForge.Application.DTOs
{
    public class MessageCreatedEventDto
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string CorrelationId { get; set; }

    }
}
