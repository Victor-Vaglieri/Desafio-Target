using System;
using System.Globalization;

namespace CalculoJuros
{
    class Program
    {
        static void Main()
        {
            // Solicitar ao usuario o valor do titulo e a data de vencimento com validao de entrada
            Console.Write("Informe o valor do titulo: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valor) || valor <= 0)
            {
                Console.WriteLine("Valor invalido.");
                return;
            }

            Console.Write("Informe a data de vencimento (ex: dd/mm/aaaa, dd-mm-aaaa, aaaa-mm-dd): ");
            string[] formatosData = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "yyyy/MM/dd" };
            
            if (!DateTime.TryParseExact(Console.ReadLine(), formatosData, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataVencimento))
            {
                Console.WriteLine("Data invalida. Certifique-se de inserir a data em um dos formatos suportados.");
                return;
            }

            DateTime dataAtual = DateTime.Today;
            int diasAtraso = (dataAtual - dataVencimento).Days;

            if (diasAtraso <= 0) // Se o pagamento n�o est� em atraso
            {
                Console.WriteLine("O pagamento nao esta em atraso.");
                Console.WriteLine($"Valor a pagar: R$ {valor:F2}");
            }
            else // Se o pagamento esta em atraso calcular juros
            {
                decimal taxaDiaria = 0.025m;
                decimal juros = CalcularJuros(valor, diasAtraso, taxaDiaria);
                decimal valorTotal = CalcularValorTotal(valor, juros);

                Console.WriteLine($"Dias de atraso: {diasAtraso}");
                Console.WriteLine($"Valor dos juros: R$ {juros:F2}");
                Console.WriteLine($"Valor total a pagar: R$ {valorTotal:F2}");
            }
        }


        // Calcula o valor dos juros com base no valor original, dias de atraso e taxa diaria
        static decimal CalcularJuros(decimal valor, int diasAtraso, decimal taxaDiaria)
        {
            return valor * taxaDiaria * diasAtraso;
        }

        // Calcula o valor total a pagar com base no valor original e os juros
        static decimal CalcularValorTotal(decimal valor, decimal juros)
        {
            return valor + juros;
        }
    }
}
