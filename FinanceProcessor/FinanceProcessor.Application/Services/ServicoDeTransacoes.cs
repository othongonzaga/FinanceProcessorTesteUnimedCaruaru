using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Application.Interfaces;
using FinanceProcessor.FinanceProcessor.Domain.Entities;
using FinanceProcessor.FinanceProcessor.Domain.Enums;
using FinanceProcessor.FinanceProcessor.Domain.Exceptions;

namespace FinanceProcessor.FinanceProcessor.Application.Services
{
    public class ServicoDeTransacoes : IServicoDeTransacoes
    {
        private readonly IRepositorioContas _repositorio;

        public ServicoDeTransacoes(IRepositorioContas repositorio)
        {
            _repositorio = repositorio;
        }

        public Guid AbrirConta(string agencia, string numeroConta, Moeda moeda, decimal saldoInicial = 0)
        {
            var conta = new Conta(agencia, numeroConta, moeda, saldoInicial);
            _repositorio.Adicionar(conta);
            return conta.IdConta;
        }

        public decimal ConsultarSaldo(Guid idConta)
        {
            var conta = ObterConta(idConta);
            return conta.SaldoAtual;
        }

        public IEnumerable<string> ObterExtrato(Guid idConta, DateTime inicio, DateTime fim)
        {
            var conta = ObterConta(idConta);
            return conta.HistoricoDeTransacoes
                .Where(t => t.DataHora >= inicio && t.DataHora <= fim)
                .OrderBy(t => t.DataHora)
                .Select(t => $"{t.DataHora:u} - {t.Tipo} - {t.Valor} {t.Moeda} - {t.Descricao}");
        }

        public void RegistrarCredito(Guid idConta, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null, string? categoria = null)
        {
            var conta = ObterConta(idConta);
            ValidarMoeda(conta, moeda);
            var transacao = new Transacao(dataHora, TipoTransacao.Credito, valor, moeda, descricao, categoria);
            conta.AdicionarTransacao(transacao);
            _repositorio.Atualizar(conta);
        }

        public void RegistrarDebito(Guid idConta, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null, string? categoria = null)
        {
            var conta = ObterConta(idConta);
            ValidarMoeda(conta, moeda);

            if (conta.SaldoAtual < valor)
                throw new SaldoInsuficienteException(conta.NumeroConta, conta.SaldoAtual, valor);

            var transacao = new Transacao(dataHora, TipoTransacao.Debito, valor, moeda, descricao, categoria);
            conta.AdicionarTransacao(transacao);
            _repositorio.Atualizar(conta);
        }

        public void RealizarTransferencia(Guid idOrigem, Guid idDestino, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null)
        {
            var contaOrigem = ObterConta(idOrigem);
            var contaDestino = ObterConta(idDestino);

            ValidarMoeda(contaOrigem, moeda);
            ValidarMoeda(contaDestino, moeda);

            if (contaOrigem.SaldoAtual < valor)
                throw new SaldoInsuficienteException(contaOrigem.NumeroConta, contaOrigem.SaldoAtual, valor);

            var debito = new Transacao(dataHora, TipoTransacao.TransferenciaEnviada, valor, moeda, descricao);
            var credito = new Transacao(dataHora, TipoTransacao.TransferenciaRecebida, valor, moeda, descricao);

            contaOrigem.AdicionarTransacao(debito);
            contaDestino.AdicionarTransacao(credito);

            _repositorio.Atualizar(contaOrigem);
            _repositorio.Atualizar(contaDestino);
        }

        public IEnumerable<string> ListarTransacoesPorTipo(Guid idConta, TipoTransacao tipo, DateTime inicio, DateTime fim)
        {
            var conta = ObterConta(idConta);
            return conta.HistoricoDeTransacoes
                .Where(t => t.Tipo == tipo && t.DataHora >= inicio && t.DataHora <= fim)
                .OrderBy(t => t.DataHora)
                .Select(t => $"{t.DataHora:u} - {t.Valor:C} {t.Moeda} - {t.Descricao}");
        }

        public decimal CalcularSaldoEmDataEspecifica(Guid idConta, DateTime data)
        {
            var conta = ObterConta(idConta);
            var transacoesAteData = conta.HistoricoDeTransacoes
                .Where(t => t.DataHora <= data.ToUniversalTime());

            decimal saldo = 0;
            foreach (var t in transacoesAteData)
            {
                saldo += t.Tipo switch
                {
                    TipoTransacao.Credito => t.Valor,
                    TipoTransacao.TransferenciaRecebida => t.Valor,
                    TipoTransacao.Debito => -t.Valor,
                    TipoTransacao.TransferenciaEnviada => -t.Valor,
                    TipoTransacao.PagamentoBoleto => -t.Valor,
                    _ => 0
                };
            }

            return saldo;
        }

        public string? EncontrarTransacaoMaisValiosa(Guid idConta, TipoTransacao tipo, DateTime inicio, DateTime fim)
        {
            var conta = ObterConta(idConta);
            var transacao = conta.HistoricoDeTransacoes
                .Where(t => t.Tipo == tipo && t.DataHora >= inicio && t.DataHora <= fim)
                .OrderByDescending(t => t.Valor)
                .FirstOrDefault();

            return transacao != null
                ? $"{transacao.DataHora:u} - {transacao.Valor:C} {transacao.Moeda} - {transacao.Descricao}"
                : null;
        }

        private Conta ObterConta(Guid idConta)
        {
            return _repositorio.ObterPorId(idConta) ?? throw new ContaNaoEncontradaException(idConta);
        }

        private void ValidarMoeda(Conta conta, Moeda moeda)
        {
            if (conta.MoedaPadrao != moeda)
                throw new TransacaoInvalidaException($"Moeda da transação ({moeda}) diferente da moeda da conta ({conta.MoedaPadrao}).");
        }
    }
}
