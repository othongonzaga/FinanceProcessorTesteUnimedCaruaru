using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Domain.Enums;

namespace FinanceProcessor.FinanceProcessor.Application.Interfaces
{
    public interface IServicoDeTransacoes
    {
        Guid AbrirConta(string agencia, string numeroConta, Moeda moeda, decimal saldoInicial = 0);
        decimal ConsultarSaldo(Guid idConta);
        IEnumerable<string> ObterExtrato(Guid idConta, DateTime inicio, DateTime fim);
        void RegistrarCredito(Guid idConta, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null, string? categoria = null);
        void RegistrarDebito(Guid idConta, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null, string? categoria = null);
        void RealizarTransferencia(Guid idOrigem, Guid idDestino, decimal valor, Moeda moeda, DateTime dataHora, string? descricao = null);
        IEnumerable<string> ListarTransacoesPorTipo(Guid idConta, TipoTransacao tipo, DateTime inicio, DateTime fim);
        decimal CalcularSaldoEmDataEspecifica(Guid idConta, DateTime data);
        string? EncontrarTransacaoMaisValiosa(Guid idConta, TipoTransacao tipo, DateTime inicio, DateTime fim);

    }
}
