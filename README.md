# Projeto XYZ

## Descrição

Breve descrição do seu projeto aqui.

## Clonando o Repositório

Para clonar este repositório, execute o seguinte comando:

```sh
git clone https://github.com/seu-usuario/seu-repositorio.git
Configuração
Adicione as configurações da AWS no arquivo appsettings.json no seguinte formato:

json
Copiar código
{
  "AWS": {
    "Profile": "default",
    "Region": "us-east-2",
    "AccessKey": "AKIAQ3EGUHI35J6XDVOQ",
    "SecretKey": "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",
    "BucketName": "mottucnh"
  }
}
Executando a Aplicação
Para subir os containers da aplicação e do banco de dados, execute:

sh
Copiar código
docker-compose up --build
Conexão com o Banco de Dados
A conexão padrão do PostgreSQL está configurada para rodar no Docker:

plaintext
Copiar código
Host=postgres;Port=5432;Username=postgres;Password=postgres;Database=mottu
Caso deseje rodar localmente, será necessário alterar esta configuração.

Acesso ao Swagger
Acesse o Swagger pelo seguinte URL:

http://localhost:8081/swagger/index.html

Atualizando o Banco de Dados
Para atualizar o banco de dados, acesse a rota:

bash
Copiar código
api/Migrations/update
Autenticação
Faça login utilizando as credenciais abaixo:

json
Copiar código
{
  "userName": "adm",
  "password": "adm"
}
Pegue o token recebido e adicione no header de autorização com o formato:

Copiar código
Bearer {token}
Rotas Disponíveis
Rotas Livres de Autenticação:

Login
Criar Deliveryman
Exemple Controller
Atualizar Migrations
Rotas Acessíveis Apenas pelo ADM:
