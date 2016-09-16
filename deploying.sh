#!/bin/bash

# Script de deploy - Projeto Base Laravel

FILE_NAME='package.tar.gz'
HOST='ftp.prodeb.gov.br'
USER='homologasar'
PASSWD='hsar@258'
DESTIN_DIR='/test'

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

write() { echo -e "\e[36m$1\e[0m"; }
error() { echo -e "\e[31m$1\e[0m"; }
success() { echo -e "\e[38;5;34m$1\e[0m"; }

success ':::: Iniciando procedimento ::::'

SEND_TO_FTP=true

echo "Deseja, no final, enviar o pacote gerado para o FTP?"
select yn in "sim" "nao"; do
    case $yn in
        sim ) SEND_TO_FTP=true; break;;
        nao ) SEND_TO_FTP=false; break;;
        *) error "Opção inválida. Digite 1 para sim ou 2 para não.";continue;;
    esac
done

write '\nCopiando arquivos'

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

write 'Executando comandos do php artisan para limpeza do cache e logs\n'

php artisan cache:clear
php artisan route:clear
php artisan view:clear
php artisan config:clear
php artisan clear-compiled

## Removendo os arquivos de log, sessions e cache
removeFileOrDir storage/logs/*
removeFileOrDir storage/framework/sessions/*
removeFileOrDir storage/framework/cache/*

## Otimizando o projeto
php artisan optimize

write '\nExecutando gulp para minificar js e css\n'

## Gerando os arquivos minificados .js e .css
gulp --production

write '\nRemovendo fontes do js e css'

## Removendo os arquivos .js
rm -rf public/client/app/*.js
rm -rf public/client/app/**/*.js

write 'Removendo diretorio de exemplos'

## Removendo o diretório de exemplos
removeFileOrDir public/client/app/samples

write 'Zipando'

## Criando o pacote zipado do projeto
tar -zcf $FILE_NAME server.php artisan app/ bootstrap/ config/ public/ resources/ storage/ vendor/ .env

## Copiando o pacote para a pasta raiz do projeto
cp $FILE_NAME ../

cd ../

write 'Removendo pasta deploy(temporaria)'

## Removendo o diretório de deploy
removeFileOrDir deploy

if [ "$SEND_TO_FTP" = true ] ; then

write 'Enviando para FTP'

## Transfere para o FTP

ftp -n $HOST <<END_SCRIPT
quote USER $USER
quote PASS $PASSWD
cd $DESTIN_DIR
put $FILE_NAME
quit
END_SCRIPT

fi

success "\n:::: $FILE_NAME gerado com sucesso! ::::"

exit 0
