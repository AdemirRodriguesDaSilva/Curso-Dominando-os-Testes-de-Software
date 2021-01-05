using NerdStore.Core.Data;
using System;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.Pedidos.Interface
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        void AdicionarItem(PedidoItem pedidoItem);
        void AtualizarItem(PedidoItem pedidoItem);
        Task<Pedido> ObterPedidoRascunho(Guid clienteId);
    }
}