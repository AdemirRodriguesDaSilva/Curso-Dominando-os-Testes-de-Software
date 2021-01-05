using NerdStore.Core.DomainObjects.Enums;
using NerdStore.Vendas.Core.DomainObjects;
using NerdStore.Vendas.Domain.Pedidos;
using NerdStore.Vendas.Domain.Pedidos.Factorys;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests.Pedidos
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicinar Novo Item Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        //Explicação sobre a construção do nome do método
        //AdicionarItemPedido: NOME DO MÉTODO
        //NovoPedido: ESTADO
        //DeveAtualizarValor: CARACTECTIA (COMPORTAMENTO DO MÉTODO)
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(2, 100);

            //Act
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesESomarValores()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(produtoId, 2, 100);
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem(produtoId, 1, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItens.Count);
            Assert.Equal(3, pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente com unidades acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemJaExistenteSomarUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(produtoId, 1, 100);
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem(produtoId, Pedido.MAXIMO_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoItemNaoExisteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = PedidoItemFactory.NovoPedidoItem();

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoItemValido_DeveAtualizarAQuantidadeDeItemDoProduto()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidotItem = PedidoItemFactory.NovoPedidoItem(produtoId);
            var peditoItemAtualizado = PedidoItemFactory.AtualizarUnidadesNoPedidoItemJaExistente(pedidotItem, 5);
            pedido.AdicionarItem(pedidotItem);

            // Act 
            pedido.AtualizarItem(peditoItemAtualizado);

            // Assert
            Assert.Equal(peditoItemAtualizado.Quantidade, pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidotItemExistente1 = PedidoItemFactory.NovoPedidoItem(2, 100);
            var pedidotItemExistente2 = PedidoItemFactory.NovoPedidoItem(3, 15);
            pedido.AdicionarItem(pedidotItemExistente1);
            pedido.AdicionarItem(pedidotItemExistente2);

            var pedidoItemAtualizado = PedidoItemFactory.AtualizarUnidadesNoPedidoItemJaExistente(pedidotItemExistente2, 5);
            var valorTotalPedido = pedidotItemExistente1.Quantidade * pedidotItemExistente1.ValorUnitario +
                                   pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            // Act 
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(valorTotalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComUnidadesAcimaDoPermitido_DeveRetornarExeption()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(3);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = PedidoItemFactory.AtualizarUnidadesNoPedidoItemJaExistente(pedidoItem, Pedido.MAXIMO_UNIDADES_ITEM);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComUnidadesAbaixoDoPermitido_DeveRetornarExeption()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(Pedido.MINIMO_UNIDADES_ITEM);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = PedidoItemFactory.AtualizarUnidadesNoPedidoItemJaExistente(pedidoItem, Pedido.MINIMO_UNIDADES_ITEM - 1);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve estar na listar para ser removido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoFoiAdicionadoNoPedido_DeveRetornarException()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = PedidoItemFactory.NovoPedidoItem();

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItem));
        }

        [Fact(DisplayName = "Remover Item Pedido Atualizar valor total do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotalPedido()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = PedidoItemFactory.NovoPedidoItem();
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem();
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var valorTotalPedido = pedidoItem1.Quantidade * pedidoItem1.ValorUnitario;

            // Act 
            pedido.RemoverItem(pedidoItem2);

            // Assert
            Assert.Equal(valorTotalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DevemRetornarSemErros()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            // Act 
            var resultado = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(resultado.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DevemRetornarSemErros()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, false);

            // Act 
            var resultado = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(resultado.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher Tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVaucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = PedidoItemFactory.NovoPedidoItem(1, 20);
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem(1, 100);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var totalPedidoComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            // Act 
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(totalPedidoComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher Tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVaucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = PedidoItemFactory.NovoPedidoItem(1, 20);
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem(1, 100);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-REAIS", null, 10, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(10), true, false);

            var totalPedidoComDesconto = pedido.ValorTotal - (pedido.ValorTotal * voucher.PercentualDesconto.Value) / 100;

            // Act 
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(totalPedidoComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher Desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVaucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(1, 100);
            pedido.AdicionarItem(pedidoItem);

            var voucher = new Voucher("PROMO-200-REAIS", 200, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            // Act 
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher Recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVaucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedido = PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = PedidoItemFactory.NovoPedidoItem(1, 100);
            pedido.AdicionarItem(pedidoItem1);

            var voucher = new Voucher("PROMO-10-REAIS", 10, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);
            pedido.AplicarVoucher(voucher);
                        
            var pedidoItem2 = PedidoItemFactory.NovoPedidoItem(1, 250);

            // Act 
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            var totalEsperado = pedido.PedidoItens.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, pedido.ValorTotal);
        }
    }
}