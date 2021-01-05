using FluentValidation.Results;
using NerdStore.Core.DomainObjects.Enums;
using NerdStore.Vendas.Core.DomainObjects;
using NerdStore.Vendas.Domain.Pedidos.Validations;
using System;

namespace NerdStore.Vendas.Domain.Pedidos
{
    public class Voucher : Entity
    {
        public string Codigo { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public Voucher(string codigo, decimal? valorDesconto, decimal? percentualDesconto, int quantidade, TipoDescontoVoucher tipoDescontoVoucher, DateTime dataValidade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            ValorDesconto = valorDesconto;
            PercentualDesconto = percentualDesconto;
            Quantidade = quantidade;
            TipoDescontoVoucher = tipoDescontoVoucher;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }
}