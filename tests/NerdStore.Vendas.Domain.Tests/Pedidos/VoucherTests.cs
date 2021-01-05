using NerdStore.Core.DomainObjects.Enums;
using NerdStore.Vendas.Domain.Pedidos;
using NerdStore.Vendas.Domain.Pedidos.Validations;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests.Pedidos
{
    public class VoucherTests
    {
        //Explicação sobre a construção do nome do método
        //Voucher: NOME DO MÉTODO
        //ValidarVoucherValor: ESTADO
        //DeveEstarValido: Caracteristica (COMPORTAMENTO DO MÉTODO)
        [Fact(DisplayName = "Validar Voucher Tipo valor válido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            // Act
            var resultado = voucher.ValidarSeAplicavel();

            //Assert
            Assert.True(resultado.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Tipo valor inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher(string.Empty, null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            // Act
            var resultado = voucher.ValidarSeAplicavel();
            int totalErrosNaCriacaoDoVoucher = 6;

            //Assert
            Assert.False(resultado.IsValid);
            Assert.Equal(totalErrosNaCriacaoDoVoucher, resultado.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.ValorDescontoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher Tipo pocentagem válido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-REAIS", null, 10, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            // Act
            var resultado = voucher.ValidarSeAplicavel();

            //Assert
            Assert.True(resultado.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Tipo pocentagem inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher(string.Empty, null, null, 0, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act
            var resultado = voucher.ValidarSeAplicavel();
            int totalErrosNaCriacaoDoVoucher = 6;

            //Assert
            Assert.False(resultado.IsValid);
            Assert.Equal(totalErrosNaCriacaoDoVoucher, resultado.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.PercentualDescontoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, resultado.Errors.Select(e => e.ErrorMessage));
        }
    }
}