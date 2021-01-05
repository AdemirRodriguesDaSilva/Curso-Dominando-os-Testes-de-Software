﻿using FluentValidation.Results;
using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; protected set; }
        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();
    }
}