using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceProcessor.FinanceProcessor.Domain.Exceptions
{
    public class TransacaoInvalidaException : Exception
    {
        public TransacaoInvalidaException(string mensagem) : base(mensagem) { }
    }
}
