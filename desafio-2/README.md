# Desafio 2: Controle de Estoque

## Enunciado do Desafio
Faça um programa onde seja possível lançar movimentações de estoque dos produtos pré-carregados (via JSON), dando **entrada** ou **saída** da mercadoria no depósito. Cada movimentação deve obrigatoriamente possuir:

* Um número **identificador único**.
* Uma **descrição** para identificar o tipo e motivo da movimentação realizada.

Ao final da movimentação, o programa deve retornar a **quantidade final (atualizada)** do estoque do produto movimentado.

## Execução

O projeto Console independente e pode ser executado através do .NET CLI.

```
cd .\desafio-1\
dotnet run
```


## Funcionamento e Testes

Abaixo, está detalhado como o sistema de estoque opera na prática através do terminal. O sistema possuí um banco de dados relacional **SQLite** para persistir de fato todas as informações.

### 1. Menu Principal
Ao rodar a aplicação, o usuário é apresentado a um menu com opções de gerenciar as movimentações ou consultar o histórico de auditoria do estoque.

![Print do terminal mostrando o Menu Principal de opções do programa, exibindo escolhas como "Registrar Movimentação" e "Consultar Histórico"](https://github.com/Victor-Vaglieri/Desafio-Target/blob/main/images/img1-desafio2.png)

### 2. Registrar Movimentação (Entrada/Saída)
O sistema pedirá o código do produto. Em seguida, solicita o tipo de operação (Entrada ou Saída), a descrição detalhada e a quantidade. Após inserir os dados, uma transação é salva no banco SQLite e a nova quantidade de estoque é exibida em tela.

![Print do terminal mostrando o usuário digitando o código do produto, inserindo a operação, descrição e quantidade, e por fim o sistema retornando o "Movimentação ID" gerado e o "Estoque atualizado" final.](https://github.com/Victor-Vaglieri/Desafio-Target/blob/main/images/img2-desafio2.png)

### 3. Consultar Histórico de Movimentações
Como uma feature adicional, foi desenvolvido um painel de histórico. O usuário pode informar o código de um produto específico (ou listar tudo) e ver todo o fluxo logado com carimbo de data/hora, tipo de alteração, estoques anterior/posterior e a descrição associada.

![Print do terminal mostrando a lista do histórico de movimentações (trazendo o ID único, Produto, Data/Hora, Quantidade alterada e saldo anterior e novo)](https://github.com/Victor-Vaglieri/Desafio-Target/blob/main/images/img3-desafio2.png)
