using MediatR;
using Moq;
using Moq.AutoMock;
using NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem;
using NerdStore.Vendas.Domain.Pedidos;
using NerdStore.Vendas.Domain.Pedidos.Factorys;
using NerdStore.Vendas.Domain.Pedidos.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        private readonly AdicionarPedidoItemCommandHandler _adicionarPedidoItemCommandHandler;

        public PedidoCommandHandlerTests()
        {
            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();

            _pedido = PedidoFactory.NovoPedidoRascunho(_clienteId);

            _mocker = new AutoMocker();
            _adicionarPedidoItemCommandHandler = _mocker.CreateInstance<AdicionarPedidoItemCommandHandler>();
        }

        [Fact(DisplayName = "Adicionar Novo Item Pedido com sucesso")]
        [Trait("Categoria", "Vendas - Adicionar Pedido Item Command Handler")]
        public async Task AdicionarItem_NovoItemPedido_DeveExecutarComSucesso()
        {
            // Arrage
            var adicionarPedidoItemCommand = new AdicionarPedidoItemCommand(_clienteId, _produtoId, "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var resultado = await _adicionarPedidoItemCommandHandler.Handle(adicionarPedidoItemCommand, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //_mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Novo Item Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Adicionar Pedido Item Command Handler")]
        public async Task AdicionarItem_NovoItemPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItem = PedidoItemFactory.NovoPedidoItem();
            _pedido.AdicionarItem(pedidoItem);

            var adicionarPedidoItemCommand = new AdicionarPedidoItemCommand(_clienteId, Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunho(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var resultado = await _adicionarPedidoItemCommandHandler.Handle(adicionarPedidoItemCommand, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Existente ao Pedido com sucesso")]
        [Trait("Categoria", "Vendas - Adicionar Pedido Item Command Handler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange           
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(_produtoId);
            _pedido.AdicionarItem(pedidoItem);

            var adicionarPedidoItemCommand = new AdicionarPedidoItemCommand(_clienteId, _produtoId, "Produto Teste", 1, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunho(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var resultado = await _adicionarPedidoItemCommandHandler.Handle(adicionarPedidoItemCommand, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Adicionar Pedido Item Commnad Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var adicionarPedidoItemCommand = new AdicionarPedidoItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

            // Act
            var resultado = await _adicionarPedidoItemCommandHandler.Handle(adicionarPedidoItemCommand, CancellationToken.None);

            // Assert
            Assert.False(resultado);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}