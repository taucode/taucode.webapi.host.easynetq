﻿using DemoCommon;
using System;
using TauCode.Cqrs.Mq;
using TauCode.Domain.Events;
using TauCode.Mq.Abstractions;

namespace AppHost
{
    public class DomainEventConverter : IDomainEventConverter
    {
        public IMessage Convert(IDomainEvent domainEvent)
        {
            if (domainEvent is SendMessageDomainEvent sendMessageDomainEvent)
            {
                return SendMessageMessage.FromDomainEvent(sendMessageDomainEvent);
            }

            if (domainEvent is PingDomainEvent pingDomainEvent)
            {
                return PingMessage.FromDomainEvent(pingDomainEvent);
            }

            throw new NotSupportedException();
        }
    }
}
