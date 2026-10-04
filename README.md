# Desafios Target Sistemas

**Desafios Target Sistemas** é um conjunto de soluções (Desafios 1, 2 e 3) desenvolvidas em C# (.NET) para demonstrar proficiência em lógica de programação, arquitetura de software, e integração com banco de dados. Os desafios abrangem desde cálculos de comissão de vendas até um sistema completo de controle de estoque com persistência de dados.

## 1. Visão Geral

O projeto foi dividido em três desafios, cada um focando em um aspecto prático e técnico de desenvolvimento de software:
* **Desafio 1 (Comissões):** Leitura de dados (JSON) e processamento lógico para geração de relatório agrupado e ordenado de comissões de vendas.
* **Desafio 2 (Controle de Estoque):** Sistema interativo em terminal para registro e consulta de entradas/saídas de mercadorias, incluindo persistência relacional e histórico de movimentações.
* **Desafio 3 (Cálculo de Juros):** Algoritmo de cálculo de juros diários sobre atrasos, com foco em modularização e tratamento de validação de datas.

## 2. Tecnologias e Ferramentas (Stack)

* **Linguagem Principal:** C# (.NET)
* **Processamento de Dados:** `System.Text.Json` (Deserialização e manipulação de JSON)
* **Banco de Dados (Desafio 2):** SQLite (`Microsoft.Data.Sqlite`) utilizado para persistência local e relacional.
* **Paradigma:** Programação Orientada a Objetos (POO) e uso de LINQ para manipulação de coleções.

## 3. Estrutura do Projeto

```text
desafio Target/
├── desafio 1/            # Sistema de Cálculo de Comissões (JSON, LINQ)
├── desafio 2/            # Sistema de Controle de Estoque (SQLite, CRUD)
├── desafio 3/            # Sistema de Cálculo de Juros
└── README.md             # Documentação principal
```

## 4. Execução

Cada desafio é um projeto Console independente e pode ser executado através do .NET CLI.

### Passo a Passo

1. **Executar o Desafio 1 (Cálculo de Comissões):**
    ```bash
    cd .\desafio-1\
    dotnet run
    ```
2. **Executar o Desafio 2 (Controle de Estoque):**
    ```bash
    cd .\desafio-2\
    dotnet run
    ```
3. **Executar o Desafio 3 (Cálculo de Juros):**
    ```bash
    cd .\desafio-3\
    dotnet run
    ```

## 5. Motivação e Escolhas Arquiteturais

* **Tipagem Forte e LINQ (Desafio 1):** O retorno foi tipado via `RelatorioComissao` usando LINQ. Isso foi feito para garantir integridade e facilitar o envio futuro desses dados para uma API, ao invés de usar loops de impressão simples.
* **Persistência com SQLite (Desafio 2):** O desafio **não pedia** banco de dados, mas um SQLite (`estoque.db`) foi implementado. Isso foi feito para demonstrar proficiência prática com persistência de dados real e consultas relacionais (SQL).
* **Histórico de Movimentações (Desafio 2):** A função extra de consultar o log (histórico) foi adicionada. Isso foi feito para simular o comportamento de um sistema de estoque auditável.
* **Modularização (Desafio 3):** A lógica matemática de juros foi separada das interações de entrada e saída em funções próprias. Isso foi feito para escalar a aplicação e adicionar novas lógicas de taxas sem comprometer o loop principal, em resumo, uma boa prática.
