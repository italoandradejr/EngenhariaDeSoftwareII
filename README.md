Loja de Cupcakes - Projeto Integrador II (Engenharia de Software)
Este é um projeto acadêmico desenvolvido para a disciplina de "Projeto Integrador Transdisciplinar em Engenharia de Software II". A aplicação consiste em um e-commerce (loja virtual) completo para uma loja de cupcakes, construído com C# .NET Core e seguindo o padrão arquitetural MVC.
O projeto foi planejado na disciplina anterior (Engenharia de Software I) e executado nesta, cobrindo o ciclo de vida completo do desenvolvimento, desde a revisão dos requisitos até os testes e a validação.
Funcionalidades (Histórias de Usuário Implementadas)
O sistema possui três perfis de usuário (Visitante, Cliente e Administrador) e implementa as seguintes funcionalidades:
    Visitante
•	HU01: Visualizar a vitrine de cupcakes com produtos carregados do banco de dados.
•	HU02: Adicionar/Remover itens em um carrinho de compras (localStorage).
•	HU07: Remover itens do carrinho.
•	HU08: Ver o resumo do pedido no carrinho.
    Cliente
•	HU04: Cadastrar-se como um novo cliente (com validação de e-mail único e criptografia de senha).
•	HU04: Fazer login e logout no sistema.
•	HU03: Finalizar um pedido (apenas se estiver logado). O pedido é salvo no banco de dados.
•	HU06: Visualizar o histórico de pedidos anteriores em uma página de "Meus Pedidos".
    Administrador
•	HU05: Fazer login em uma área restrita (/Admin/Login) com credenciais fixas (admin@cupcake.com / admin123).
•	HU10: Cadastrar novos cupcakes, que aparecem imediatamente na vitrine.
•	HU11: Remover cupcakes existentes do sistema.
•	HU12: Visualizar um painel de gerenciamento com os produtos cadastrados.
________________________________________

Tecnologias Utilizadas
•	Back-end: C# com .NET Core 6 (ou superior)
•	Arquitetura: MVC (Model-View-Controller)
•	Banco de Dados: MySQL
•	ORM: Entity Framework Core (com driver Pomelo.EntityFrameworkCore.MySql)
•	Front-end: HTML5, CSS3, JavaScript (ES6) e Bootstrap 5
•	Gerenciamento de Carrinho: localStorage do navegador
•	Autenticação: Autenticação via Cookies do ASP.NET Core
•	Segurança: BCrypt.Net-Next (para hashing de senhas de clientes)
•	Testes: xUnit (para testes unitários de back-end)
________________________________________
Como Executar o Projeto Localmente
Siga estes passos para configurar e rodar a aplicação em sua máquina local.
    1. Pré-requisitos
•	Visual Studio 2022 (ou .NET Core SDK 6+)
•	MySQL Workbench (ou outro gerenciador de banco de dados MySQL)
•	Um servidor MySQL local em execução.
    2. Configuração do Banco de Dados
1.	No seu servidor MySQL, crie um novo banco de dados (schema) chamado dbcupcakes.
2.	Abra o arquivo MySQL_Script_Completo.sql (ou o script fornecido no histórico) no MySQL Workbench.
3.	Execute o script para criar todas as tabelas (Cupcakes, Clientes, Pedidos, etc.) e popular a tabela Cupcakes com os produtos iniciais.
    3. Configuração da Aplicação C#
1.	Abra o projeto (.sln) no Visual Studio.
2.	Abra o arquivo appsettings.json.
3.	Modifique a ConnectionStrings para apontar para o seu banco MySQL local, inserindo seu usuário e senha:
JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=dbcupcakes;User=SEU_USUARIO_MYSQL;Password=SUA_SENHA_MYSQL;"
}
4.	Instale os pacotes NuGet necessários (clique com o botão direito na solução > "Restaurar Pacotes NuGet"). Os principais são:
o	Pomelo.EntityFrameworkCore.MySql
o	BCrypt.Net-Next
o	Microsoft.EntityFrameworkCore.Tools
    4. Executar
1.	Pressione F5 ou clique no botão "Play" (IIS Express) no Visual Studio para compilar e iniciar a aplicação.
2.	O navegador será aberto na página da vitrine.
________________________________________
🔑 Acesso de Administrador
•	URL: /Admin/Login
•	Email: admin@cupcake.com
•	Senha: admin123

