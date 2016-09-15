#!/bin/bash

# Script de deploy - Projeto Base Laravel

removeFileOrDir() {
  if [ -d "$1" ] || [ -f "$1" ];
  then
      rm -rf $1
  fi
}

copy() {
    if [ ! -d "$2" ]
    then
        mkdir -p $2
    fi
    cp -r $1 $2
}

copyEnv() {
    if [ ! -f .env.production ]
    then
        echo "O arquivo .env.production não foi encontrado."
        exit
    else
        copy .env.production deploy
        mv "deploy/.env.production" "deploy/.env"
    fi
}

## Copiando o arquivo .env.production
copyEnv

## Copiando o arquivo artisan
copy artisan deploy

## Copiando os diretórios e arquivos necessários para produção
copy app deploy
copy bootstrap deploy
copy config deploy
copy public deploy
copy resources deploy
copy storage deploy
copy vendor deploy
copy artisan deploy
copy server.php deploy
copy gulpfile.js deploy
copy bower_components deploy

## Acessando a pasta deploy
cd deploy/

## Limpando o cache do projeto
php artisan cache:clear
php artisan route:clear
php artisan view:clear
php artisan config:clear
php artisan auth:clear-resets
php artisan clear-compiled

## Otimizando o projeto
php artisan optimize

## Gerando os arquivos minificados .js e .css
gulp --production

## Removendo os arquivos de log, sessions e cache
removeFileOrDir storage/logs/*
removeFileOrDir storage/framework/sessions/*
removeFileOrDir storage/framework/cache/*

## Removendo os arquivos .js
rm -rf public/client/app/*.js
rm -rf public/client/app/**/*.js

## Removendo o diretório de exemplos
removeFileOrDir public/client/app/samples

## Criando o pacote zipado do projeto
tar -cf deploy.tar server.php artisan app/ bootstrap/ config/ public/ resources/ storage/ vendor/ .env
gzip -9 deploy.tar

## Copiando o pacote para a pasta raiz do projeto
cp deploy.tar.gz ../

cd ../

## Removendo o diretório de deploy
removeFileOrDir deploy

echo '\n:::: Pacote deploy.tar.gz gerado com sucesso! ::::\n'

exit
