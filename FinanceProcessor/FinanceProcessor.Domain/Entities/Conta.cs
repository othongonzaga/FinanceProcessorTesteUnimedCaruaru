using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Domain.Enums;

namespace FinanceProcessor.FinanceProcessor.Domain.Entities
{
    public class Conta
    {
        public Guid IdConta { get; private set; } = Guid.NewGuid();
        public string NumeroAgencia { get; private set; }
        public string NumeroConta { get; private set; }
        public Moeda MoedaPadrao { get; private set; }
        public decimal SaldoAtual { get; private set; }

        private readonly List<Transacao> _historicoTransacoes = new();
        public IReadOnlyCollection<Transacao> HistoricoDeTransacoes => _historicoTransacoes.AsReadOnly();

        public Conta(string numeroAgencia, string numeroConta, Moeda moedaPadrao, decimal saldoInicial = 0)
        {
            if (saldoInicial < 0)
                throw new ArgumentException("Saldo inicial não pode ser negativo.");

            NumeroAgencia = numeroAgencia;
            NumeroConta = numeroConta;
            MoedaPadrao = moedaPadrao;
            SaldoAtual = saldoInicial;
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _historicoTransacoes.Add(transacao);
            AtualizarSaldo(transacao);
        }

        private void AtualizarSaldo(Transacao transacao)
        {
            switch (transacao.Tipo)
            {
                case TipoTransacao.Credito:
                case TipoTransacao.TransferenciaRecebida:
                    SaldoAtual += transacao.Valor;
                    break;
                case TipoTransacao.Debito:
                case TipoTransacao.PagamentoBoleto:
                case TipoTransacao.TransferenciaEnviada:
                    SaldoAtual -= transacao.Valor;
                    break;
            }
        }
    }

}
