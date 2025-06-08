using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceProcessor.FinanceProcessor.Domain.Enums
{
    public enum TipoTransacao
    {
        Credito,
        Debito,
        TransferenciaEnviada,
        TransferenciaRecebida,
        PagamentoBoleto
    }
}
