using System;

namespace NerdStore.Vendas.Domain.Pedidos.Factorys
{
    public static class PedidoItemFactory
    {
        public static PedidoItem NovoPedidoItem()
        {
            return new PedidoItem(Guid.NewGuid(), "Produto Teste", 1, 100);
        }

        public static PedidoItem NovoPedidoItem(int quantidade)
        {
            return new PedidoItem(Guid.NewGuid(), "Produto Teste", quantidade, 100);
        }

        public static PedidoItem NovoPedidoItem(int quantidade, decimal valorUnitario)
        {
            return new PedidoItem(Guid.NewGuid(), "Produto Teste", quantidade, valorUnitario);
        }

        public static PedidoItem NovoPedidoItem(Guid produtoId)
        {
            return new PedidoItem(produtoId, "Produto Teste", 1, 100);
        }

        public static PedidoItem AtualizarUnidadesNoPedidoItemJaExistente(PedidoItem pedidoItemExistente, int quantidade)
        {
            pedidoItemExistente.AtualizarUnidades(quantidade);
            return pedidoItemExistente;
        }

        public static PedidoItem NovoPedidoItem(Guid produtoId, int quantidade, decimal valorUnitario)
        {
            return new PedidoItem(produtoId, "Produto Teste", quantidade, valorUnitario);
        }
    }
}