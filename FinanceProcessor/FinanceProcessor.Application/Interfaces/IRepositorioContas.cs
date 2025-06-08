using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Domain.Entities;

namespace FinanceProcessor.FinanceProcessor.Application.Interfaces
{
    public interface IRepositorioContas
    {
        Conta? ObterPorId(Guid id);
        void Adicionar(Conta conta);
        void Atualizar(Conta conta);
        IEnumerable<Conta> ListarTodas();
    }
}
