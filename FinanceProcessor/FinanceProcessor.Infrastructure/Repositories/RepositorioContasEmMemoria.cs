using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Application.Interfaces;
using FinanceProcessor.FinanceProcessor.Domain.Entities;

namespace FinanceProcessor.FinanceProcessor.Infrastructure.Repositories
{
    public class RepositorioContasEmMemoria : IRepositorioContas
    {
        private readonly Dictionary<Guid, Conta> _contas = new();

        public void Adicionar(Conta conta)
        {
            _contas[conta.IdConta] = conta;
        }

        public Conta? ObterPorId(Guid id)
        {
            _contas.TryGetValue(id, out var conta);
            return conta;
        }

        public void Atualizar(Conta conta)
        {
            _contas[conta.IdConta] = conta;
        }

        public IEnumerable<Conta> ListarTodas() => _contas.Values;
    }
}
