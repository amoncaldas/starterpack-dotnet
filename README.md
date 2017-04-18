- dotnet watch run
Inicia o servidor

- dotnet restore
Carrega as bibliotecas adicionadas

- dotnet ef migrations add NameOfMigration
Cria uma migration para refletir o estado dos models

- dotnet ef migrations remove
Apaga a última migration gerada

- dotnet ef migrations script
Gera um arquivo .sql referente ao conteudo das migrations

- dotnet ef database update
Roda as migrations

http://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/