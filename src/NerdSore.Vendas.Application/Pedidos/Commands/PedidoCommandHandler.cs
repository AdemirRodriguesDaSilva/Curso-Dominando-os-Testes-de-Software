using MediatR;
using NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem;
using NerdStore.Vendas.Domain.Pedidos.Factorys;
using NerdStore.Vendas.Domain.Pedidos.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace NerdSore.Vendas.Application.Pedidos.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarPedidoItemCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarPedidoItemCommand message, CancellationToken cancellationToken)
        {

            var pedido = PedidoFactory.NovoPedidoRascunho(message.ClientId);
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(message.ProdutoId, message.Quantidade, message.ProdutoNome, message.ValorUnitario);
            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.ProdutoNome,
                                                                       message.ValorUnitario, message.Quantidade));
            return await _pedidoRepository.UnitOfWork.Commit();
        }
    }
}