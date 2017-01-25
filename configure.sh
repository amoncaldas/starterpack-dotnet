#!/bin/bash

while true ; do
  read -p "Esse projeto já foi configurado anteriormente? (s/n)? " sn
  case $sn in
    [Ss]* ) NEW_PROJECT=false; break;;
    [Nn]* ) NEW_PROJECT=true; break;;
    *) error "Opção inválida. Digite (S) para sim ou (N) para não.";;
  esac
done

echo

if [ "$NEW_PROJECT" = true ]
then
  while getopts :u option
  do
          case "${option}"
          in
                  u) GIT_URL=${OPTARG};;
          esac
  done

  if [ -z $GIT_URL ]; then
      echo "É obrigatório fornecer a url do repositório git através da opção -u. Ex: sh configure.sh -u git@git.prodeb.ba.gov.br:nome-do-repositorio.git"
      exit
  fi

  # Configurando o GIT
  rm -rf .git
  git init
  git remote add origin $GIT_URL
  git remote add starter-pack git@git.prodeb.ba.gov.br:starter-pack/laravel_angular_base.git

  # Preparand oos arquivos templates
  cp public/client/paths.json.example-laravel public/client/paths.json
  cp public/client/app/app.global.js.example-laravel public/client/app/app.global.js
  rm -rf public/client/app/app.global.js.example.* public/client/paths.json.example.*

  sed -i '/paths.json/d' public/client/.gitignore
  sed -i '/app\/app.global.js/d' public/client/.gitignore
fi

# Configurando o Projeto

composer global require "laravel/installer=~1.1"

# Instalando as dependencias locais yoman gulp, eslint, bower e derivados
npm install -g yo gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower protractor@4.0.14 protractor-console

# Atualizando o webdriver para rodar os testes e2e
webdriver-manager update

# Instalando o gerador
npm install -g git+ssh://git@git.prodeb.ba.gov.br:starter-pack/generator-ngprodeb.git

# Dando permissão nas pastas do laravel
chmod 777 -R storage
chmod 777 -R bootstrap/cache

# Instalando as dependencias
COMPOSER_PROCESS_TIMEOUT=2000 composer install

# Gerando as chaves
php artisan key:generate
php artisan jwt:secret

# Gerando os dados de usuários padrão no banco
php artisan migrate --seed

# Dando permissão executar os scripts bash
chmod +x deploy.sh
chmod +x e2e-test.sh
chmod +x unit-test.sh

# Instalando as dependencias do frontend
cd public/client
npm install
bower install

