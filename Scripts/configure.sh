#!/bin/bash

echo

EXISTING_PROJECT=false

while getopts g:e option
do
        case "${option}"
        in
                g) GIT_URL=${OPTARG};;
                e) EXISTING_PROJECT=true;;
        esac
done

if [ $EXISTING_PROJECT = false ]
then
  if [ -z $GIT_URL ]; then
      echo "É obrigatório fornecer a url do repositório git através da opção -u. Ex: sh scripts/configure.sh -g git@git.prodeb.ba.gov.br:nome-do-repositorio.git"
      exit
  fi

  # Configurando o GIT
  rm -rf .git
  git init
  git remote add origin $GIT_URL

  # Preparand oos arquivos templates
  cp public/client/paths.json.example-dotnet public/client/paths.json
  cp public/client/app/app.global.js.example-dotnet public/client/app/app.global.js
  rm -rf public/client/app/app.global.js.example.* public/client/paths.json.example.*

  sed -i '/paths.json/d' public/client/.gitignore
  sed -i '/app\/app.global.js/d' public/client/.gitignore
  echo "0.0.1" > VERSION
fi

# adicionando a referência do projeto do starter pack
git remote add starter-pack git@git.prodeb.ba.gov.br:starter-pack/starter-pack-dotnet.git

# Configurando o Projeto

# Instalando as dependencias locais yoman gulp, eslint, bower e derivados
npm install -g yo gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower protractor protractor-console

# Atualizando o webdriver para rodar os testes e2e
webdriver-manager update

# Instalando o gerador
npm install -g git+ssh://git@git.prodeb.ba.gov.br:starter-pack/generator-ngprodeb.git

# Dando permissão nas pastas do laravel

# Instalando as dependencias

dotnet restore

# Gerando as chaves

# Gerando os dados de usuários padrão no banco
dotnet ef database update

# Instalando as dependencias do frontend
cd public/client
npm install
bower install

# Voltando para pasta raiz
cd ../..

if [ $EXISTING_PROJECT = false ]
then
  # Adicionando ao stage area. Não commita automaticamente pois pode estar dentro do docker
  git add .
fi



