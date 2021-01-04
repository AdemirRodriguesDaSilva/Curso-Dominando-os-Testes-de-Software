using NerdStore.Vendas.Core.DomainObjects;
using NerdStore.Vendas.Domain.Pedidos;
using NerdStore.Vendas.Domain.Pedidos.Factorys;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests.Pedidos
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo Item Pedido com unidades abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")] //Nome da categoria baseada no contexto que é vendas
        //Explicação sobre a construção do nome do método
        //AdicionarItemPedido: NOME DO MÉTODO
        //UnidadesItemAbaixoDoPermitido: ESTADO
        //DeveAtualizarValor: CARACTERISTICA (COMPORTAMENTO DO MÉTODO)
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange Act & Assert
            Assert.Throws<DomainException>(() => PedidoItemFactory.NovoPedidoItem(Pedido.MINIMO_UNIDADES_ITEM - 1));
        }

        [Fact(DisplayName = "Novo Item Pedido com unidades acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange Act & Assert
            Assert.Throws<DomainException>(() => PedidoItemFactory.NovoPedidoItem(Pedido.MAXIMO_UNIDADES_ITEM + 1));
        }
    }
}