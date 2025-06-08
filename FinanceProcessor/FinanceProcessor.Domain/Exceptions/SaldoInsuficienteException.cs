using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceProcessor.FinanceProcessor.Domain.Exceptions
{
    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException(string numeroConta, decimal saldoAtual, decimal valorTentativa)
            : base($"Saldo insuficiente na conta {numeroConta}. Saldo atual: {saldoAtual:C}, tentativa: {valorTentativa:C}.") { }
    }
}
