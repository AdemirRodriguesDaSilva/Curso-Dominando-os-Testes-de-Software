using NerdStore.Core.DomainObjects.Enums;
using System;

namespace NerdStore.Vendas.Domain.Pedidos.Factorys
{
    public static class PedidoFactory
    {
        public static Pedido NovoPedidoRascunho(Guid clienteId)
        {
            var pedido = new Pedido(clienteId, 0, PedidoStatus.Rascunho);
            return pedido;
        }
    }
}