# Desafio 1: Cálculo de Comissões

## Enunciado do Desafio
Considerando que um JSON (`vendas.json`) possui registros de vendas de um time comercial, o objetivo é fazer um programa que leia os dados e calcule a comissão de cada vendedor, seguindo a seguinte regra para cada venda:

* Vendas abaixo de **R$ 100,00** não geram comissão.
* Vendas abaixo de **R$ 500,00** geram **1%** de comissão.
* Vendas a partir de **R$ 500,00** geram **5%** de comissão.

O programa deve retornar as comissões de cada vendedor, devidamente agrupadas, somadas e ordenadas por ordem alfabética do nome do vendedor.

## Funcionamento e Testes

Abaixo é demonstrado a execução da aplicação lendo o conjunto de dados em JSON e aplicando a regra de negócios.

### 1. Leitura e Cálculo
Ao iniciar, o programa fará o parse do arquivo `vendas.json`, fará os cálculos de comissão de maneira agrupada por vendedor, e retornará o total que cada vendedor tem a receber.

![Print do terminal mostrando a lista de vendedores e o valor da comissão formatado em Reais (R$) de forma alfabética](images/img1-desafio1.png)

*Observação: A listagem final é fortemente tipada utilizando a estrutura `RelatorioComissao` para garantir a integridade dos dados e facilitar o escalonamento do sistema.*
