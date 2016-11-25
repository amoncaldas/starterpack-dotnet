#!/bin/bash

# Script de deploy - Starter Pack

EXTENSION='.zip'
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

copyHtaccess() {
  if [ ! -f .htaccess.example ]
  then
      error "O arquivo .htaccess.example não foi encontrado."
      exit
  else
      copy .htaccess.example
      mv "deploy/.htaccess.example" "deploy/.htaccess"
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

addInFileZip() {
  for ORIGIN in $@
  do
    zip -u -r -q $PACKAGE_NAME $ORIGIN
  done
}

# Permissão de manipulação da pasta
sudo chown $(whoami):$(whoami) . -R

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

## Copiando o arquivo .htaccess.exemple e renomeando para .htaccess
copyHtaccess

## Verificando se as pastas e arquivos obrigratórios existem
verifyIfFileExists artisan server.php public/client/gulpfile.js
verifyIfDirExists app bootstrap config public resources storage vendor public/client/bower_components

## Copiando os diretórios e arquivos necessários para produção
copy app bootstrap config public resources storage vendor artisan public/client/bower_components server.php public/client/gulpfile.js

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
gulp --gulpfile public/client/gulpfile.js --production

write '\nRemovendo fontes do js e css...'

## Removendo os arquivos .js e .css
removeFileOrDir public/client/app/*.js public/client/app/**/*.js public/client/app/**/**/*.js public/client/styles/*.scss

write '\nRemovendo diretorio de exemplos...'

## Removendo o diretório de exemplos
removeFileOrDir public/client/app/samples

## Carregando dados do arquivo .env
loadingEnvFile

PACKAGE_NAME=$PKG_NAME$EXTENSION

write '\nZipando...'

# Permissão de manipulação da pasta
sudo chown $(whoami):$(whoami) . -R

## Criando o pacote zipado do projeto
zip -rq $PACKAGE_NAME .env .htaccess
#addInFileZip artisan server.php
addInFileZip resources/ app/ bootstrap/ config/ storage/ vendor/

## Atualizando o pacote zipado
addInFileZip public/index.php public/.htaccess public/robots.txt public/client/index.html
addInFileZip public/client/app/ public/client/build/ public/client/images/ public/client/styles/

write '\nCopiando pacote para a pasta raiz do projeto...'

## Copiando o pacote para a pasta raiz do projeto
cp $PACKAGE_NAME ../

cd ../

write '\nRemovendo pasta deploy(temporaria)...'

## Removendo o diretório de deploy
removeFileOrDir $DEPLOY_DIR

if [ "$SEND_TO_FTP" = true ] ; then

## Lendo os dados de FTP do arquivo .env
getDadosFTP

write '\nEnviando para FTP...'

## Transfere para o FTP

ftp -p -n $FTP_HOST<<END_SCRIPT
quote USER $FTP_USER
quote PASS $FTP_PASSWD
cd $FTP_DIR
delete .htaccess
type binary
put $PACKAGE_NAME
put install.php
quit
END_SCRIPT

removeFileOrDir $PACKAGE_NAME

success "\n:::: $PACKAGE_NAME enviado para o FTP com sucesso! ::::"

write "\n Instalando...\n"

curl $APP_URL"/install.php?pkgName="$PACKAGE_NAME"&url="$APP_URL"&dir="$FTP_DIR | php

write "\n Removendo "$PACKAGE_NAME

ftp -p -n $FTP_HOST <<END_SCRIPT
quote USER $FTP_USER
quote PASS $FTP_PASSWD
cd $FTP_DIR
delete $PACKAGE_NAME
delete install.php
quit
END_SCRIPT

success "\n:::: $APP_NAME instalado com sucesso! ::::"

write "\n Abrindo navegador...\n"

if which xdg-open > /dev/null
then
  xdg-open $APP_URL
elif which gnome-open > /dev/null
then
  gnome-open $APP_URL
fi

else

success "\n:::: $PACKAGE_NAME gerado com sucesso! ::::\n"

fi

exit 0
