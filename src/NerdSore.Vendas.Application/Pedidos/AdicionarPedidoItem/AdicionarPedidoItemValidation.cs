using FluentValidation;
using NerdSore.Vendas.Application.Pedidos.AdicionarPedidoItem;
using NerdStore.Vendas.Domain.Pedidos;
using System;

namespace NerdSore.Vendas.Application.Pedidos.Validations
{
    public class AdicionarPedidoItemValidation : AbstractValidator<AdicionarPedidoItemCommand>
    {
        public static string ClientIdErroMsg => "Id do client inválido";
        public static string ProdutoIdErroMsg => "Id do produto inválido";
        public static string ProdutoNomeErroMsg => "O nome do produto não foi informado";
        public static string QuantidadeMaximaErroMsg => $"A quantidade máxima de um item é {Pedido.MAXIMO_UNIDADES_ITEM}";
        public static string QuantidadeMinimaErroMsg => $"A quantidade mínima de um item é {Pedido.MINIMO_UNIDADES_ITEM}";
        public static string ValorUnitarioErroMsg => "O valor do item precisa ser maios que 0";

        public AdicionarPedidoItemValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErroMsg);

            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProdutoIdErroMsg);

            RuleFor(c => c.ProdutoNome)
                .NotEmpty()
                .WithMessage(ProdutoNomeErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeMinimaErroMsg)
                .LessThanOrEqualTo(Pedido.MAXIMO_UNIDADES_ITEM)
                .WithMessage(QuantidadeMaximaErroMsg);

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0)
                .WithMessage(ValorUnitarioErroMsg);
        }
    }
}