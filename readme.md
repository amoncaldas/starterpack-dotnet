# Sobre

- Este projeto tem como objetivo servir de base para futuros projetos da Prodeb.
- O mesmo utiliza Laravel 5.1 com Angular 1.5.
- O sistema utiliza [JWT](http://jwt.io) para autenticação através da lib [tymon/jwt-auth](https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam json contendo as informações da requisitadas.

# Pré requisitos

- Preferencialmente utilize o Linux com o gerenciador APT
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
- php versão 5.6 ou superior [tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)
- extenções php_fileinfo, php_mbstring, php_pdo_pgsql, php_pgsql, php_openssl
- composer [tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)
- editor decente [vscode](https://code.visualstudio.com/) ou [atom.io](https://atom.io/)
- postgres

# Configuração

> Rode os comandos abaixo.
> Todos os comandos devem ser executados no terminal do linux. No caso do windows utilize o Git Bash.

- git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
- cd laravel_angular_base
- composer global require "laravel/installer=~1.1"
- npm install
- chmod **777** -R storage
- chmod **777** -R bootstrap/cache
- cp .env.example .env

> Configure o .env com os dados da conexão do postgres

- composer install
- php artisan key:generate
- php artisan jwt:secret
- php artisan migrate --seed

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente 

- gulp
  - parametros opcionais 
    - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )

> Em outra aba do terminal.

- npm run server (Este comando inicia o servidor php na porta 5000)

> Abra o navegador

- acesse http://localhost:5000
- logue com os dados email: **admin-base@prodeb.com** pass: **Prodeb01**

> Outros comandos

- gulp check (verifica a formatação do código javascript) 
  - parametros opcionais 
    - **--fix** (para corrigir os erros que podem ser corrigidos automaticamente)
- npm run package (prepara a aplicação para produção, minificando os arquivos js, css e modificando o index.html para apontar para os arquivos minificados)


### Erro no watch no linux

- echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p

> Para ver os logs

- acessar a url http://localhost:5000/developer/log-viewer
- digitar o usuário conforme a variável de ambiente no arquivo .env DEVELOP_ID
- digitar a senha conforme a variável de ambiente no arquivo .env DEVELOP_PASSWORD
