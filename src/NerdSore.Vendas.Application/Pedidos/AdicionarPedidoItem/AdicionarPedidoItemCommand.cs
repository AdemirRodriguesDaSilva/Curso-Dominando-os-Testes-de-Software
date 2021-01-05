using NerdSore.Vendas.Application.Pedidos.Validations;
using NerdStore.Core.Messages;
using System;

namespace NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem
{
    public class AdicionarPedidoItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AdicionarPedidoItemCommand(Guid clientId, Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ClientId = clientId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }


        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}