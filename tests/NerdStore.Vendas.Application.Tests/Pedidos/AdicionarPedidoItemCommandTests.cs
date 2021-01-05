using NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem;
using NerdSore.Vendas.Application.Pedidos.Validations;
using NerdStore.Vendas.Domain.Pedidos;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarPedidoItemCommandTests
    {
        [Fact(DisplayName ="Adicionar Item Command válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarPedidoItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act
            var resultado = pedidoCommand.EhValido();

            // Assert
            Assert.True(resultado);
        }

        [Fact(DisplayName = "Adicionar Item Command inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarPedidoItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

            // Act
            var resultado = pedidoCommand.EhValido();

            // Assert
            Assert.False(resultado);
            Assert.Contains(AdicionarPedidoItemValidation.ClientIdErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarPedidoItemValidation.ProdutoIdErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarPedidoItemValidation.ProdutoIdErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarPedidoItemValidation.QuantidadeMinimaErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarPedidoItemValidation.ValorUnitarioErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Adicionar Item Command inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_QuantidadeDeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarPedidoItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", Pedido.MAXIMO_UNIDADES_ITEM + 1, 100);

            // Act
            var resultado = pedidoCommand.EhValido();

            // Assert
            Assert.False(resultado);
            Assert.Contains(AdicionarPedidoItemValidation.QuantidadeMaximaErroMsg, pedidoCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}