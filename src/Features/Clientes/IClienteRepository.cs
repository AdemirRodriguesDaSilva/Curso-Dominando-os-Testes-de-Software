using Features.Core;

namespace Features.Clientes
{
    public interface IClienteRepository : IRepositoryTesteDeUnidade<Cliente>
    {
        Cliente ObterPorEmail(string email);
    }
}