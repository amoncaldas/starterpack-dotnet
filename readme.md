# Sobre

- Este projeto tem como objetivo servir de base para futuros projetos da Prodeb.
- O mesmo utiliza Laravel 5.1 com Angular 1.5.
- O sistema utiliza JWT (http://jwt.io) para autenticação através da lib (https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam json contendo as informações da requisitadas.

# Pré requisitos

- linux (preferência ubuntu)
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
- php versão 5.6 ou superior [tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)
- composer [tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)
- editor decente [atom.io](https://atom.io/)
- postgres

## Para desenvolvimento

> abra o terminal e rode os comandos abaixo

- git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
- cd laravel_angular_base
- composer global require "laravel/installer=~1.1"
- npm install -g gulp bower
- npm install
- bower install
- chmod **777** -R storage
- chmod **777** -R bootstrap/cache
- cp .env.example .env

> configure o .env com os dados da conexão do postgres

- composer install
- php artisan key:generate
- php artisan migrate
- php artisan db:seed
- gulp

> em outra aba do terminal

- php artisan serve --port=5000

> abra o navegador

- acesse localhost:5000
- logue com os dados email: admin-base@prodeb.com pass: Prodeb01
