using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EventBus
{
    public interface IEventHandler
    {
        public interface IEventHandler
        {
        }

        public class EventHandlerBase : IEventHandler
        {
            public Contexts.DefaultDbContext Db { protected get; set; }

            public IMediator Bus { protected get; set; }

            public ILogger<EventHandlerBase> Logger { protected get; set; }
        }
    }
}
