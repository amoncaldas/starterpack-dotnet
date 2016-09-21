#!/bin/bash

# Script de deploy - Projeto Base Laravel

EXTENSION='.tar.gz'
DEPLOY_DIR='deploy'

## Color Block
RED='\033[0;31m'
GREEN='\033[0;32m'
BCYAN='\033[1;36m'
NO_COLOR='\033[0m'

## Funcoes que printam as mensagens no console
write() { echo "${NO_COLOR}$1${NO_COLOR}"; }
error() { echo "${RED}Error: ${1}${NO_COLOR}"; }
success() { echo "${BCYAN}$1${NO_COLOR}"; }

createDeployDir() {
  if [ ! -d "$DEPLOY_DIR" ]
  then
      mkdir -p $DEPLOY_DIR
  fi
}

removeFileOrDir() {
  for ORIGIN in $@ ; do
    if [ -d "$ORIGIN" ] || [ -f "$ORIGIN" ] ;
    then
        rm -rf $ORIGIN
    fi
  done
}

copy() {
  for ORIGIN in $@
  do
    cp -r $ORIGIN $DEPLOY_DIR
  done
}

copyEnv() {
  if [ ! -f .env.production ]
  then
      error "O arquivo .env.production não foi encontrado."
      exit
  else
      copy .env.production
      mv "deploy/.env.production" "deploy/.env"
  fi
}

executePHPArtisan() {
  for ORIGIN in $@
  do
    php artisan $ORIGIN
  done
}

exitError() {
  error "$1"
  removeFileOrDir $DEPLOY_DIR $PACKAGE_NAME
  exit
}

verifyIfFileExists() {
  for ORIGIN in $@ ; do
    if [ ! -f "$ORIGIN" ] ;
    then
        exitError "O arquivo (${ORIGIN}) não foi encontrado, não é possível prosseguir sem ele."
    fi
  done
}

verifyIfDirExists() {
  for ORIGIN in $@ ; do
    if [ ! -d "$ORIGIN" ] ;
    then
        exitError "O diretório (${ORIGIN}) não foi encontrado, não é possível prosseguir sem ele."
    fi
  done
}

loadingEnvFile() {
  . ./.env
}

getDadosFTP() {
  write '\nAcessando dados de FTP no arquivo .env...'

  if [ -z ${FTP_HOST+x} ] ;
  then
    exitError "\nFTP_HOST no arquivo .env.production não foi definido.\n"
  fi

  if [ -z ${FTP_USER+x} ] ;
  then
    exitError "\nFTP_USER no arquivo .env.production não foi definido.\n"
  fi

  if [ -z ${FTP_PASSWD+x} ] ;
  then
    exitError "\nFTP_PASSWD no arquivo .env.production não foi definido.\n"
  fi

  if [ -z ${FTP_DIR+x} ] ;
  then
    exitError "\nFTP_DIR no arquivo .env.production não foi definido.\n"
  fi

}

# Cria o diretorio temporario(deploy)
createDeployDir

success ':::: Iniciando Procedimento ::::'

SEND_TO_FTP=true

while true ; do
  read -p "Deseja, no final, enviar o pacote gerado para o FTP (s/n)? " sn
  case $sn in
    [Ss]* ) SEND_TO_FTP=true; break;;
    [Nn]* ) SEND_TO_FTP=false; break;;
    *) error "Opção inválida. Digite S para sim ou N para não.";;
  esac
done

write '\nCopiando arquivos...'

## Copiando o arquivo .env.production e renomeando para .env
copyEnv

## Verificando se as pastas e arquivos obrigratórios existem
verifyIfFileExists artisan server.php gulpfile.js
verifyIfDirExists app bootstrap config public resources storage vendor bower_components

## Copiando os diretórios e arquivos necessários para produção
copy app bootstrap config public resources storage vendor artisan bower_components server.php gulpfile.js

## Acessando a pasta deploy
cd $DEPLOY_DIR/

write '\nExecutando comandos do php artisan para limpeza do cache e logs...\n'

## Limpando cache e logs do laravel
executePHPArtisan cache:clear route:clear view:clear config:clear clear-compiled

write '\nRemovendo os arquivos de log, sessions e cache...'

## Removendo os arquivos de log, sessions e cache
removeFileOrDir storage/logs/* storage/framework/sessions/* storage/framework/cache/*

write '\nOtimizando o projeto...\n'

## Otimizando o projeto
executePHPArtisan optimize

write '\nExecutando gulp para minificar js e css...'

## Gerando os arquivos minificados .js e .css
gulp --production

write '\nRemovendo fontes do js e css...'

## Removendo os arquivos .js e .css
removeFileOrDir public/client/app/*.js public/client/app/**/*.js public/client/app/**/**/*.js public/client/styles/*.scss

write '\nRemovendo diretorio de exemplos...'

## Removendo o diretório de exemplos
removeFileOrDir public/client/app/samples

## Carregando dados do arquivo .env
loadingEnvFile

PACKAGE_NAME=$APP_NAME$EXTENSION

write '\nZipando...'

## Criando o pacote zipado do projeto
tar -zcf $PACKAGE_NAME server.php artisan app/ bootstrap/ config/ public/ resources/ storage/ vendor/ .env

write '\nCopiando pacote para a pasta raiz do projeto...'

## Copiando o pacote para a pasta raiz do projeto
cp $PACKAGE_NAME ../

cd ../

if [ "$SEND_TO_FTP" = true ] ; then

## Lendo os dados de FTP do arquivo .env
getDadosFTP

write '\nEnviando para FTP...'

## Transfere para o FTP

ftp -n $FTP_HOST <<END_SCRIPT
quote USER $FTP_USER
quote PASS $FTP_PASSWD
cd $FTP_DIR
put $PACKAGE_NAME
quit
END_SCRIPT

fi

write '\nRemovendo pasta deploy(temporaria)...'

## Removendo o diretório de deploy
removeFileOrDir $DEPLOY_DIR

success "\n:::: $PACKAGE_NAME gerado com sucesso! ::::\n"

exit 0
