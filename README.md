# TorneioManager

## Descrição

O TorneioManager é uma aplicação web para gerenciar torneios, incluindo a criação de torneios, registro de participantes e gerenciamento de partidas. A aplicação é desenvolvida com ASP.NET Core, Entity Framework Core e SQLite.

## Funcionalidades

- **Criação de Torneios**: Permite a criação de novos torneios com um nome e uma lista de participantes.
- **Gerenciamento de Partidas**: As partidas são geradas automaticamente com base nos participantes registrados e são gerenciadas ao longo das rodadas do torneio.
- **Definição de Vencedores**: Permite definir o vencedor de cada partida, atualizando o progresso do torneio até a finalização.
- **Visualização de Detalhes**: Exibe os detalhes de cada torneio, incluindo participantes e partidas por rodada.

## Estrutura do Projeto

### Controllers

- **TournamentsController**: Controlador principal responsável por gerenciar as ações relacionadas aos torneios.

### Services

- **TournamentService**: Serviço que encapsula a lógica de negócio para gerenciamento de torneios, incluindo a geração de partidas e definição de vencedores.

### Models

- **Tournament**: Modelo que representa um torneio.
- **Participant**: Modelo que representa um participante do torneio.
- **Match**: Modelo que representa uma partida no torneio.

### Data

- **ApplicationDbContext**: Contexto do banco de dados que gerencia a interação com o SQLite usando Entity Framework Core.

## Configuração

### Pré-requisitos

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQLite](https://www.sqlite.org/index.html)

### Passos para Configuração

1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/TorneioManager.git
    ```

2. Navegue até o diretório do projeto:
    ```bash
    cd TorneioManager
    ```

3. Restaure as dependências do projeto:
    ```bash
    dotnet restore
    ```

4. Atualize o banco de dados:
    ```bash
    dotnet ef database update
    ```

5. Execute a aplicação:
    ```bash
    dotnet run
    ```

6. Acesse a aplicação em seu navegador:
    ```
    http://localhost:5000
    ```

## Uso

### Criação de Torneio

1. Navegue até a página de criação de torneio.
2. Insira o nome do torneio.
3. Adicione os participantes.
4. Clique em "Create".

### Gerenciamento de Partidas

1. Na página de detalhes do torneio, visualize as partidas agrupadas por rodada.
2. Para definir o vencedor de uma partida, selecione o vencedor na lista suspensa e clique em "Set Winner".

### Visualização de Detalhes

1. Na página principal, clique em "View Details" ao lado de um torneio para visualizar os participantes e partidas.

## Estrutura de Código

### TournamentsController.cs

Controlador que gerencia as ações relacionadas aos torneios, incluindo listagem, detalhes, criação e definição de vencedores.

### TournamentService.cs

Serviço que encapsula a lógica de negócio para o gerenciamento de torneios.

### Views

- **Create.cshtml**: Página de criação de torneio.
- **Details.cshtml**: Página de detalhes do torneio, incluindo a lista de participantes e partidas.
- **Index.cshtml**: Página principal com a listagem de torneios.

## Melhorias Futuras

- **Autenticação e Autorização**: Implementar autenticação e autorização para restringir o acesso a determinadas ações.
- **Notificações em Tempo Real**: Usar SignalR para atualizações em tempo real das partidas.
- **Melhorias na UI/UX**: Melhorar a interface do usuário para uma experiência mais amigável.

## Contribuição

Contribuições são bem-vindas! Para contribuir, siga os passos abaixo:

1. Fork o repositório.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`).
4. Push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.


