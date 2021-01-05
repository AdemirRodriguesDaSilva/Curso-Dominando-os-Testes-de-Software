using MediatR;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Core.DomainObjects
{
    public class DomainNotification : Message, INotification
    {
        public Guid Id { get; private set; }
        public DateTime Data { get; private set; }
        public string Chave { get; private set; }
        public string Valor { get; private set; }
        public int Versao { get; private set; }

        public DomainNotification(string chave, string valor)
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            Chave = chave;
            Valor = valor;
            Versao = 1;
        }
    }
}