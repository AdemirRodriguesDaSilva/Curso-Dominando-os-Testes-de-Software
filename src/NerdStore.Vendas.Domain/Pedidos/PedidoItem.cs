using NerdStore.Vendas.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain.Pedidos
{
    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            Validar(produtoId, produtoNome, quantidade, valorUnitario);

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;

        }

        private void Validar(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            if (quantidade > Pedido.MAXIMO_UNIDADES_ITEM)
                throw new DomainException($"Máximo de {Pedido.MAXIMO_UNIDADES_ITEM} unidades por produto");

            if (quantidade < Pedido.MINIMO_UNIDADES_ITEM)
                throw new DomainException($"Mínimo de {Pedido.MINIMO_UNIDADES_ITEM} unidades por produto");
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal void AtualizarUnidades(int unidades)
        {
            Quantidade = unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}