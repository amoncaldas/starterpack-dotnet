# Inicia o servidor
- dotnet watch run

# Carrega as bibliotecas adicionadas
- dotnet restore

# Cria uma migration para refletir o estado dos models
- dotnet ef migrations add NameOfMigration

# Apaga a última migration gerada
- dotnet ef migrations remove

# Gera um arquivo .sql referente ao conteudo das migrations
- dotnet ef migrations script

# Roda as migrations
- dotnet ef database update
- dotnet ef database update -e Local

# Para rodar os tests
- dotnet test ou dotnet xunit

# Para listar os testes
- dotnet test -t

# Para executar verificar as opções da CLI do StartPack
- dotnet run sp -h

# Para ver todas as opções do seed do CLI do StartPack
- dotnet run sp seed -h

# Para executar o seed do StartPack
- dotnet run sp seed

# Para executar o seed do CLI do StartPack resetando os dados
- dotnet run sp seed --reset

# Para executar o deploy do StartPack
- dotnet run sp deploy