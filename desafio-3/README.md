# Desafio 3: Cálculo de Juros por Atraso

## Enunciado do Desafio
Faça um programa que a partir de um valor de título e de uma data de vencimento, calcule o **valor dos juros na data de hoje**, considerando que a multa de atraso seja fixada em **2,5% ao dia**.

## Execução

O projeto Console independente e pode ser executado através do .NET CLI.

```
cd .\desafio-3\
dotnet run
```


## Funcionamento e Testes

O programa de cálculo foi construído separando a lógica matemática do I/O, o que torna o código muito escalável e fácil de receber modificações nas regras de negócio de juros no futuro. 

Abaixo temos as demonstrações de funcionamento da validação e cálculo:

### 1. Inserção de Dados e Cálculos
Ao iniciar, o programa fará a leitura do valor original e da data de vencimento (permitindo variados formatos de data). Se o título estiver vencido com relação à data de hoje, será aplicada a alíquota de juros compostos ou simples (neste caso, a multa diária de 2,5%) iterando sobre a diferença de dias.

![Print do terminal mostrando o usuário informando um valor (Ex: 1000) e uma data retroativa, em seguida o sistema exibindo a quantidade de dias em atraso e o valor final com o juros aplicado](https://github.com/Victor-Vaglieri/Desafio-Target/blob/main/images/img1-desafio3.png)

### 2. Tratamento e Validação de Título em Dia
O sistema também tem inteligência para detectar datas futuras ou na mesma data presente. Neste cenário, não há cobrança de juros e o cliente paga o valor original.

![Print do terminal mostrando o usuário informando uma data futura (não vencida) e o sistema mostrando a mensagem "O pagamento não está em atraso" com o valor a pagar igual ao original](https://github.com/Victor-Vaglieri/Desafio-Target/blob/main/images/img2-desafio3.png)
