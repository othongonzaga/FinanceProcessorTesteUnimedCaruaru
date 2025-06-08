using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceProcessor.FinanceProcessor.Domain.Enums;


namespace FinanceProcessor.FinanceProcessor.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime DataHora { get; private set; }
        public TipoTransacao Tipo { get; private set; }
        public decimal Valor { get; private set; }
        public Moeda Moeda { get; private set; }
        public string? Descricao { get; private set; }
        public string? Categoria { get; private set; }

        public Transacao(DateTime dataHora, TipoTransacao tipo, decimal valor, Moeda moeda, string? descricao = null, string? categoria = null)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor da transação deve ser positivo.");

            Id = Guid.NewGuid();
            DataHora = dataHora.ToUniversalTime();
            Tipo = tipo;
            Valor = valor;
            Moeda = moeda;
            Descricao = descricao;
            Categoria = categoria;
        }
    }
}
