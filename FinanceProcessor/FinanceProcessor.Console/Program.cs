using FinanceProcessor.FinanceProcessor.Application.Interfaces;
using FinanceProcessor.FinanceProcessor.Application.Services;
using FinanceProcessor.FinanceProcessor.Domain.Enums;
using FinanceProcessor.FinanceProcessor.Infrastructure.Repositories;

var repositorio = new RepositorioContasEmMemoria();
IServicoDeTransacoes servico = new ServicoDeTransacoes(repositorio);

Console.WriteLine("Sistema de Processamento Financeiro Avançado");

bool continuar = true;
while (continuar)
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine("1 - Abrir Conta");
    Console.WriteLine("2 - Consultar Saldo");
    Console.WriteLine("3 - Registrar Crédito");
    Console.WriteLine("4 - Registrar Débito");
    Console.WriteLine("5 - Realizar Transferência");
    Console.WriteLine("6 - Obter Extrato");
    Console.WriteLine("7 - Listar Transações por Tipo");
    Console.WriteLine("8 - Calcular Saldo em Data Específica");
    Console.WriteLine("9 - Transação Mais Valiosa");
    Console.WriteLine("0 - Sair");

    Console.Write("> ");
    var opcao = Console.ReadLine();

    try
    {
        switch (opcao)
        {
            case "1":
                AbrirConta(servico);
                break;
            case "2":
                ConsultarSaldo(servico);
                break;
            case "3":
                RegistrarCredito(servico);
                break;
            case "4":
                RegistrarDebito(servico);
                break;
            case "5":
                RealizarTransferencia(servico);
                break;
            case "6":
                ObterExtrato(servico);
                break;
            case "7":
                ListarTransacoesPorTipo(servico);
                break;
            case "8":
                CalcularSaldoEmData(servico);
                break;
            case "9":
                TransacaoMaisValiosa(servico);
                break;

            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void AbrirConta(IServicoDeTransacoes servico)
{
    Console.Write("Agência: ");
    var ag = Console.ReadLine()!;
    Console.Write("Número da Conta: ");
    var nc = Console.ReadLine()!;
    Console.Write("Moeda (BRL/USD): ");
    var moeda = Enum.Parse<Moeda>(Console.ReadLine()!, true);
    Console.Write("Saldo Inicial: ");
    var saldo = decimal.Parse(Console.ReadLine()!);

    var id = servico.AbrirConta(ag, nc, moeda, saldo);
    Console.WriteLine($" Conta criada com ID: {id}");
}

void ConsultarSaldo(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    var saldo = servico.ConsultarSaldo(id);
    Console.WriteLine($"Saldo atual: {saldo:C}");
}

void RegistrarCredito(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Valor: ");
    var valor = decimal.Parse(Console.ReadLine()!);
    Console.Write("Moeda (BRL/USD): ");
    var moeda = Enum.Parse<Moeda>(Console.ReadLine()!, true);
    Console.Write("Descrição: ");
    var desc = Console.ReadLine();
    Console.Write("Categoria: ");
    var cat = Console.ReadLine();

    servico.RegistrarCredito(id, valor, moeda, DateTime.UtcNow, desc, cat);
    Console.WriteLine("Crédito registrado.");
}

void RegistrarDebito(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Valor: ");
    var valor = decimal.Parse(Console.ReadLine()!);
    Console.Write("Moeda (BRL/USD): ");
    var moeda = Enum.Parse<Moeda>(Console.ReadLine()!, true);
    Console.Write("Descrição: ");
    var desc = Console.ReadLine();
    Console.Write("Categoria: ");
    var cat = Console.ReadLine();

    servico.RegistrarDebito(id, valor, moeda, DateTime.UtcNow, desc, cat);
    Console.WriteLine(" Débito registrado.");
}

void RealizarTransferencia(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta Origem: ");
    var origem = Guid.Parse(Console.ReadLine()!);
    Console.Write("ID da Conta Destino: ");
    var destino = Guid.Parse(Console.ReadLine()!);
    Console.Write("Valor: ");
    var valor = decimal.Parse(Console.ReadLine()!);
    Console.Write("Moeda (BRL/USD): ");
    var moeda = Enum.Parse<Moeda>(Console.ReadLine()!, true);
    Console.Write("Descrição: ");
    var desc = Console.ReadLine();

    servico.RealizarTransferencia(origem, destino, valor, moeda, DateTime.UtcNow, desc);
    Console.WriteLine("Transferência concluída.");
}

void ObterExtrato(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Data Início (yyyy-MM-dd): ");
    var inicio = DateTime.Parse(Console.ReadLine()!);
    Console.Write("Data Fim (yyyy-MM-dd): ");
    var fim = DateTime.Parse(Console.ReadLine()!);

    var extrato = servico.ObterExtrato(id, inicio.ToUniversalTime(), fim.ToUniversalTime());

    Console.WriteLine("Extrato:");
    foreach (var linha in extrato)
    {
        Console.WriteLine($"- {linha}");
    }
}

void ListarTransacoesPorTipo(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Tipo de Transação (Credito, Debito, TransferenciaEnviada, etc): ");
    var tipo = Enum.Parse<TipoTransacao>(Console.ReadLine()!, true);
    Console.Write("Data Início (yyyy-MM-dd): ");
    var inicio = DateTime.Parse(Console.ReadLine()!);
    Console.Write("Data Fim (yyyy-MM-dd): ");
    var fim = DateTime.Parse(Console.ReadLine()!);

    var transacoes = servico.ListarTransacoesPorTipo(id, tipo, inicio.ToUniversalTime(), fim.ToUniversalTime());

    Console.WriteLine($"Transações do tipo {tipo}:");
    foreach (var linha in transacoes)
    {
        Console.WriteLine($"- {linha}");
    }
}

void CalcularSaldoEmData(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Data (yyyy-MM-dd): ");
    var data = DateTime.Parse(Console.ReadLine()!);

    var saldo = servico.CalcularSaldoEmDataEspecifica(id, data.ToUniversalTime());
    Console.WriteLine($"Saldo em {data:yyyy-MM-dd}: {saldo:C}");
}


void TransacaoMaisValiosa(IServicoDeTransacoes servico)
{
    Console.Write("ID da Conta: ");
    var id = Guid.Parse(Console.ReadLine()!);
    Console.Write("Tipo de Transação (Credito, Debito, etc): ");
    var tipo = Enum.Parse<TipoTransacao>(Console.ReadLine()!, true);
    Console.Write("Data Início (yyyy-MM-dd): ");
    var inicio = DateTime.Parse(Console.ReadLine()!);
    Console.Write("Data Fim (yyyy-MM-dd): ");
    var fim = DateTime.Parse(Console.ReadLine()!);

    var resultado = servico.EncontrarTransacaoMaisValiosa(id, tipo, inicio.ToUniversalTime(), fim.ToUniversalTime());
    if (resultado != null)
        Console.WriteLine($"Transação mais valiosa: {resultado}");
    else
        Console.WriteLine("Nenhuma transação encontrada no período.");
}
