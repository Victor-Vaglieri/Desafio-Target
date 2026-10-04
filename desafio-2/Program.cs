using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;


namespace ControleEstoque
{
    public class Produto
    {
        [JsonPropertyName("codigoProduto")]
        public int Codigo_produto { get; set; }

        [JsonPropertyName("descricaoProduto")]
        public required string Descricao_produto { get; set; }

        [JsonPropertyName("estoque")]
        public int Estoque { get; set; }
    }

    public class DadosEstoque
    {
        [JsonPropertyName("estoque")]
        public required List<Produto> Estoque { get; set; }
    }

    [JsonSerializable(typeof(DadosEstoque))]
    public partial class EstoqueJsonContext : JsonSerializerContext
    {
    }

    class Program
    {
        static string connectionString = "Data Source=estoque.db";

        static void Main()
        {
            InicializarBancoDeDados();

            while (true)
            {
                Console.WriteLine("\n--- Sistema de Estoque ---");
                Console.WriteLine("1 - Registrar Movimentacao");
                Console.WriteLine("2 - Consultar Historico de Produto");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opcao: ");

                string? opcao = Console.ReadLine();

                if (opcao == "0") break;
                else if (opcao == "1") RegistrarMovimentacao();
                else if (opcao == "2") ConsultarHistorico();
                else Console.WriteLine("Opcao invalida.");
            }
        }

        static void InicializarBancoDeDados()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Produtos (
                    CodigoProduto INTEGER PRIMARY KEY,
                    DescricaoProduto TEXT,
                    Estoque INTEGER
                );

                CREATE TABLE IF NOT EXISTS Movimentacoes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CodigoProduto INTEGER,
                    NomeProduto TEXT,
                    DataHora TEXT,
                    Tipo TEXT,
                    Quantidade INTEGER,
                    EstoqueAnterior INTEGER,
                    EstoquePosterior INTEGER,
                    Descricao TEXT,
                    FOREIGN KEY(CodigoProduto) REFERENCES Produtos(CodigoProduto)
                );
            ";
            command.ExecuteNonQuery();

            command.CommandText = "SELECT COUNT(*) FROM Produtos";
            long count = (long)(command.ExecuteScalar() ?? 0L);

            if (count == 0)
            {
                CarregarDadosIniciais(connection);
            }
        }

        static void CarregarDadosIniciais(SqliteConnection connection)
        {
            string caminho_do_arquivo = "estoque.json";
            if (!File.Exists(caminho_do_arquivo))
            {
                Console.WriteLine($"Arquivo '{caminho_do_arquivo}' nao encontrado para carga inicial.");
                return;
            }

            string json = File.ReadAllText(caminho_do_arquivo);
            var dados = JsonSerializer.Deserialize(json, EstoqueJsonContext.Default.DadosEstoque);

            if (dados?.Estoque == null)
            {
                Console.WriteLine("Arquivo JSON de estoque vazio ou invalido.");
                return;
            }

            using var transaction = connection.BeginTransaction();
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "INSERT INTO Produtos (CodigoProduto, DescricaoProduto, Estoque) VALUES ($codigo, $descricao, $estoque)";

            foreach (var produto in dados.Estoque)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("$codigo", produto.Codigo_produto);
                command.Parameters.AddWithValue("$descricao", produto.Descricao_produto);
                command.Parameters.AddWithValue("$estoque", produto.Estoque);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        static void RegistrarMovimentacao()
        {
            Console.Write("\nInforme o Codigo do Produto: ");
            if (!int.TryParse(Console.ReadLine(), out int codigo)) return;

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT DescricaoProduto, Estoque FROM Produtos WHERE CodigoProduto = $codigo";
            command.Parameters.AddWithValue("$codigo", codigo);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                Console.WriteLine("Produto nao localizado.");
                return;
            }

            string nomeProduto = reader.GetString(0);
            int estoqueAnterior = reader.GetInt32(1);
            reader.Close();

            Console.Write("Operacao (1 - Entrada, 2 - Saida): ");
            string? operacao = Console.ReadLine();
            bool isEntrada = operacao == "1";

            if (operacao != "1" && operacao != "2")
            {
                Console.WriteLine("Operacao invalida.");
                return;
            }

            Console.Write("Descricao da movimentacao: ");
            string? descricao = Console.ReadLine();

            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade invalida.");
                return;
            }

            if (!isEntrada && estoqueAnterior < quantidade)
            {
                Console.WriteLine("Operacao cancelada: Estoque insuficiente.");
                return;
            }

            int estoquePosterior = isEntrada ? estoqueAnterior + quantidade : estoqueAnterior - quantidade;

            using var transaction = connection.BeginTransaction();
            try
            {
                var updateCommand = connection.CreateCommand();
                updateCommand.Transaction = transaction;
                updateCommand.CommandText = "UPDATE Produtos SET Estoque = $estoque WHERE CodigoProduto = $codigo";
                updateCommand.Parameters.AddWithValue("$estoque", estoquePosterior);
                updateCommand.Parameters.AddWithValue("$codigo", codigo);
                updateCommand.ExecuteNonQuery();

                var insertCommand = connection.CreateCommand();
                insertCommand.Transaction = transaction;
                insertCommand.CommandText = @"
                    INSERT INTO Movimentacoes (CodigoProduto, NomeProduto, DataHora, Tipo, Quantidade, EstoqueAnterior, EstoquePosterior, Descricao)
                    VALUES ($codigo, $nome, $dataHora, $tipo, $quantidade, $estoqueAnterior, $estoquePosterior, $descricao);
                    SELECT last_insert_rowid();";
                insertCommand.Parameters.AddWithValue("$codigo", codigo);
                insertCommand.Parameters.AddWithValue("$nome", nomeProduto);
                insertCommand.Parameters.AddWithValue("$dataHora", DateTime.Now.ToString("o"));
                insertCommand.Parameters.AddWithValue("$tipo", isEntrada ? "Entrada" : "Saida");
                insertCommand.Parameters.AddWithValue("$quantidade", quantidade);
                insertCommand.Parameters.AddWithValue("$estoqueAnterior", estoqueAnterior);
                insertCommand.Parameters.AddWithValue("$estoquePosterior", estoquePosterior);
                insertCommand.Parameters.AddWithValue("$descricao", descricao);
                
                long idMov = (long)(insertCommand.ExecuteScalar() ?? 0L);

                transaction.Commit();

                Console.WriteLine($"\nMovimentacao ID: {idMov} registrada com sucesso.");
                Console.WriteLine($"Produto: {nomeProduto}");
                Console.WriteLine($"Estoque atualizado: {estoquePosterior}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Erro ao registrar movimentacao: " + ex.Message);
            }
        }

        static void ConsultarHistorico()
        {
            Console.Write("\nInforme o Codigo do Produto para consultar o historico (ou 0 para exibir todos): ");
            if (!int.TryParse(Console.ReadLine(), out int codigo)) return;

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            if (codigo == 0)
            {
                Console.WriteLine("\n--- Historico Geral de Movimentacoes ---");
                command.CommandText = "SELECT Id, CodigoProduto, NomeProduto, DataHora, Tipo, Quantidade, EstoqueAnterior, EstoquePosterior, Descricao FROM Movimentacoes ORDER BY Id";
            }
            else
            {
                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT DescricaoProduto, Estoque FROM Produtos WHERE CodigoProduto = $codigo";
                checkCommand.Parameters.AddWithValue("$codigo", codigo);
                using var reader = checkCommand.ExecuteReader();
                
                if (!reader.Read())
                {
                    Console.WriteLine("Produto nao localizado.");
                    return;
                }
                
                Console.WriteLine($"\n--- Historico de: {reader.GetString(0)} (CCodigo: {codigo}) ---");
                Console.WriteLine($"Estoque atual do produto: {reader.GetInt32(1)}");
                reader.Close();

                command.CommandText = "SELECT Id, CodigoProduto, NomeProduto, DataHora, Tipo, Quantidade, EstoqueAnterior, EstoquePosterior, Descricao FROM Movimentacoes WHERE CodigoProduto = $codigo ORDER BY Id";
                command.Parameters.AddWithValue("$codigo", codigo);
            }

            using var movReader = command.ExecuteReader();
            bool hasRows = false;
            while (movReader.Read())
            {
                hasRows = true;
                DateTime dataHora = DateTime.Parse(movReader.GetString(3));
                Console.WriteLine($"\n[{dataHora:dd/MM/yyyy HH:mm:ss}] Movimentacao ID: {movReader.GetInt64(0)} | Produto ID: {movReader.GetInt64(1)} / Nome do Produto: {movReader.GetString(2)}");
                Console.WriteLine($"Tipo: {movReader.GetString(4)} | Quantidade: {movReader.GetInt32(5)} | Descricao: {movReader.GetString(8)}");
                Console.WriteLine($"Alteracao de Estoque: {movReader.GetInt32(6)} -> {movReader.GetInt32(7)}");
            }

            if (!hasRows)
            {
                Console.WriteLine("Nenhuma movimentacao registrada.");
            }
        }
    }
}
