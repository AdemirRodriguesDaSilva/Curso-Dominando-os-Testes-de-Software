using System;

namespace NerdStore.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AgregateId { get; protected set; }

        public Message()
        {
            MessageType = GetType().Name;
        }
    }
}