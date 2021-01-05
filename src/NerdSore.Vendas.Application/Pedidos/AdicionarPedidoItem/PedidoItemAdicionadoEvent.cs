using NerdStore.Core.Messages;
using System;

namespace NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem
{
    public class PedidoItemAdicionadoEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int Quantidade { get; private set; }

        public PedidoItemAdicionadoEvent(Guid clientId, Guid pedidoId, Guid produtoId, string produtoNome, decimal valorUnitario, int quantidade)
        {
            ClientId = clientId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
        }
    }
}