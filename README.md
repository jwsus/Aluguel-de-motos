
## Clonando o Repositório
Para clonar este repositório, execute o seguinte comando: https://github.com/seu-usuario/seu-repositorio.git
## Configuração appsettings.json
Adicione as configurações da AWS no arquivo appsettings.json no seguinte formato:

``` "AWS": {
    "Profile": "default",
    "Region": "us-east-2",
    "AccessKey": "",
    "SecretKey": "",
    "BucketName": ""
  } ```
Altere o "DefaultConnection" para rodar via docker para: ```"Host=postgres;Port=5432;Username=postgres;Password=postgres;Database=mottu"```
nesse formato sua aplicação ira conseguir se conectar no seu banco de dados, caso queira rodar localmente utilize: ```"Host=localhost;Port=5432;Username=postgres;Password=root;Database=Mottu"```

## Executando a Aplicação
Para subir os containers da aplicação e do banco de dados, execute:
```docker-compose up --build```

## Acesso ao Swagger
Acesse o Swagger pelo seguinte URL:
```http://localhost:8081/swagger/index.html```

## Atualizando o Banco de Dados
Para atualizar o banco de dados, acesse a rota:
```api/Migrations/update```

Necessário atualizar o banco para prosseguir.

##Autenticação
Faça login utilizando as credenciais abaixo:
```{
  "userName": "adm",
  "password": "adm"
}```

Pegue o token recebido e adicione no header de autorização com o formato:
```Bearer {token}```

 partir daqui existem rotas livres de autenticação: ```Login, criar deliveryman, exemple controller, atualizar migrations``` e existem rotas que somente o ADM ou ENTREGADOR pode acessar e rotas que ambos podem acessar
