using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceProcessor.FinanceProcessor.Domain.Exceptions
{
    public class ContaNaoEncontradaException : Exception
    {
        public ContaNaoEncontradaException(Guid idConta)
            : base($"Conta com ID '{idConta}' não encontrada.") { }
    }
}
