#Pré requisitos

- linux (preferência ubuntu)
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
- php versão 5.6 ou superior [tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)
- composer [tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)
- editor descente [atom.io](https://atom.io/)

## Para desenvolvimento

> abra o terminal e rode os comandos abaixo

- git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
- cd laravel_angular_base
- composer global require "laravel/installer=~1.1"
- npm install -g gulp
- npm install
- chmod **777** -R storage
- chmod **777** -R bootstrap/cache
- cp .env.example .env
- composer install
- php artisan key:generate
- php artisan db:seed
- gulp

> em uma aba do terminal

- gulp watch

> em outra aba do terminal

- php artisan serve --port=5000

> abra o navegador

- acesse localhost:5000
- logue com os dados email: admin-base@prodeb.com pass: Prodeb01

## Para produção

- gulp --production

