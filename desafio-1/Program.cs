using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CalculoComissao
{
    // class Venda representa uma venda realizada por um vendedor
    public class Venda
    {
        [JsonPropertyName("vendedor")]
        public required string Vendedor { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }

    // class DadosVendas representa o conjunto de vendas
    public class DadosVendas
    {
        [JsonPropertyName("vendas")]
        public required List<Venda> Vendas { get; set; }
    }

    // Classe de transferencia de dados (DTO) introduzida para estruturar o retorno do calculo.
    public class RelatorioComissao
    {
        public required string Vendedor { get; set; }
        public decimal Total_comissao { get; set; }
    }

    [JsonSerializable(typeof(DadosVendas))]
    public partial class VendasJsonContext : JsonSerializerContext
    {
    }

    class Program
    {
        static void Main()
        {
            string caminho_do_arquivo = "vendas.json";

            if (!File.Exists(caminho_do_arquivo))
            {
                Console.WriteLine($"Arquivo '{caminho_do_arquivo}' nao encontrado. Certifique-se de cria-lo no diretorio de execucao.");
                return;
            }

            string json = File.ReadAllText(caminho_do_arquivo);

            var dados = JsonSerializer.Deserialize(json, VendasJsonContext.Default.DadosVendas);

            if (dados?.Vendas == null)
            {
                Console.WriteLine("O arquivo JSON de vendas está vazio ou inválido.");
                return;
            }

            IEnumerable<RelatorioComissao> comissoes_por_Vendedor = GerarRelatorioComissoes(dados.Vendas);

            foreach (var item in comissoes_por_Vendedor)
            {
                Console.WriteLine($"Vendedor: {item.Vendedor,-16} | Comissao Total: R$ {item.Total_comissao:F2}");
            }
        }

        // responsavel por agrupar as vendas por vendedor, calcular a comissao total de cada um e retornar uma lista ordenada de relatorios.
        static IEnumerable<RelatorioComissao> GerarRelatorioComissoes(IEnumerable<Venda> vendas)
        {
            if (vendas == null || !vendas.Any())
            {
                return Enumerable.Empty<RelatorioComissao>();
            }

            return vendas
                .GroupBy(v => v.Vendedor)
                .Select(g => new RelatorioComissao
                {
                    Vendedor = g.Key,
                    Total_comissao = g.Sum(v => Calcular_Comissao(v.Valor))
                })
                .OrderBy(relatorio => relatorio.Vendedor)
                .ToList();
        }


        // regra de comissao imposta pelo desafio: 1% para vendas abaixo de R$ 500,00 e 5% para vendas acima de R$ 500,00
        static decimal Calcular_Comissao(decimal valor)
        {
            if (valor < 100m) return 0m;
            if (valor < 500m) return valor * 0.01m;
            return valor * 0.05m;
        }
    }
}
