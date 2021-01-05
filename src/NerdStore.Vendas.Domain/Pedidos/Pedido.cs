using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.DomainObjects.Enums;
using NerdStore.Vendas.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        public static int MINIMO_UNIDADES_ITEM => 1;
        public static int MAXIMO_UNIDADES_ITEM => 15;

        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal Desconto { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public Pedido(Guid clienteId, decimal valorTotal, PedidoStatus pedidoStatus)
        {
            ClienteId = clienteId;
            ValorTotal = valorTotal;
            PedidoStatus = pedidoStatus;
            _pedidoItens = new List<PedidoItem>();
        }

        public void AdicionarItem(PedidoItem novoPedidoItem)
        {
            ValidarQuantidadePedidoItemPermitida(novoPedidoItem);

            if (PedidoItemEstaNaListaDePedidoItensAdicionados(novoPedidoItem))
            {
                var pedidoItemExistente = _pedidoItens.FirstOrDefault(i => i.ProdutoId == novoPedidoItem.ProdutoId);
                pedidoItemExistente.AdicionarUnidades(novoPedidoItem.Quantidade);
                novoPedidoItem = pedidoItemExistente;
                _pedidoItens.Remove(pedidoItemExistente);
            }

            _pedidoItens.Add(novoPedidoItem);
            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            ValidarQuantidadePedidoItemPermitida(pedidoItem);

            var pedidoItemExistente = _pedidoItens.FirstOrDefault(i => i.ProdutoId == pedidoItem.ProdutoId);
            _pedidoItens.Remove(pedidoItemExistente);
            _pedidoItens.Add(pedidoItem);
            CalcularValorPedido();
        }

        private void ValidarQuantidadePedidoItemPermitida(PedidoItem pedidoItem)
        {
            if (pedidoItem == null)
                return;

            int quantidade = pedidoItem.Quantidade;
            if (PedidoItemEstaNaListaDePedidoItensAdicionados(pedidoItem))
            {
                var pedidoItemJaAdicionado = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                quantidade += pedidoItemJaAdicionado.Quantidade;
            }

            if (quantidade > Pedido.MAXIMO_UNIDADES_ITEM)
                throw new DomainException($"Máximo de {Pedido.MAXIMO_UNIDADES_ITEM} unidades por produto");

            if (quantidade < Pedido.MINIMO_UNIDADES_ITEM)
                throw new DomainException($"Mínimo de {Pedido.MINIMO_UNIDADES_ITEM} unidades por produto");
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            _pedidoItens.Remove(pedidoItem);
            CalcularValorPedido();
        }

        public void ValidarPedidoItemInexistente(PedidoItem pedidoItem)
        {
            if (!PedidoItemEstaNaListaDePedidoItensAdicionados(pedidoItem))
                throw new DomainException($"O Item não existe no pedido");
        }

        private bool PedidoItemEstaNaListaDePedidoItensAdicionados(PedidoItem novoPedidoItem)
        {
            return _pedidoItens.Any(i => i.ProdutoId == novoPedidoItem.ProdutoId);
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItens.Sum(i => i.CalcularValor());
            CalcularDesconto();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var resultado = voucher.ValidarSeAplicavel();
            if (!resultado.IsValid)
                return resultado;

            Voucher = voucher;
            CalcularDesconto();
            return resultado;
        }

        public void CalcularDesconto()
        {
            if (Voucher == null)
                return;

            decimal desconto = 0;
            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor && Voucher.ValorDesconto.HasValue)
                desconto = Voucher.ValorDesconto.Value;
            else if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem && Voucher.PercentualDesconto.HasValue)
                desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;

            ValorTotal = desconto > ValorTotal ? 0 : ValorTotal - desconto;
            Desconto = desconto;
        }
    }
}