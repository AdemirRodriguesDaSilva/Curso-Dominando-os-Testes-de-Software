using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain.Pedidos.Factorys;
using NerdStore.Vendas.Domain.Pedidos.Interface;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem
{
    public class AdicionarPedidoItemCommandHandler : IRequestHandler<AdicionarPedidoItemCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public AdicionarPedidoItemCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarPedidoItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command))
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunho(command.ClientId);
            var pedidoItem = PedidoItemFactory.NovoPedidoItem(command.ProdutoId, command.Quantidade, command.ProdutoNome, command.ValorUnitario);

            if (pedido == null)
            {
                pedido = PedidoFactory.NovoPedidoRascunho(command.ClientId);
                pedido.AdicionarItem(pedidoItem);
                _pedidoRepository.Adicionar(pedido);
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemEstaNaListaDePedidoItensAdicionados(pedidoItem);
                pedido.AdicionarItem(pedidoItem);
                
                if (pedidoItemExistente)
                    _pedidoRepository.AtualizarItem(pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                else
                    _pedidoRepository.AdicionarItem(pedidoItem);

                _pedidoRepository.Atualizar(pedido);
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, command.ProdutoId, command.ProdutoNome,
                                                                       command.ValorUnitario, command.Quantidade));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command command)
        {
            if (command.EhValido())
                return true;

            foreach (var error in command.ValidationResult.Errors)
                _mediator.Publish(new DomainNotification(command.MessageType, error.ErrorMessage), CancellationToken.None);

            return false;
        }
    }
}