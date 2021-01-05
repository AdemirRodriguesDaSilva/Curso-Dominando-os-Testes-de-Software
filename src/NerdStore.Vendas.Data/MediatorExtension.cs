using MediatR;
using NerdStore.Vendas.Core.DomainObjects;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediator mediator, VendasContext vendasContext)
        {
            var domainEntities = vendasContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(e => e.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvents) =>
                {
                    await mediator.Publish(domainEvents);
                });

            await Task.WhenAll(tasks);
        }
    }
}