#!/bin/bash

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

## Copiando os diretórios necessários para produção
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

cd deploy/

php artisan cache:clear
php artisan route:clear
php artisan view:clear
php artisan config:clear
php artisan auth:clear-resets
php artisan clear-compiled

sed -i".bak" '23,29d' public/client/app/layout/menu.controller.js

gulp --production

rm -rf storage/logs/* storage/framework/sessions/* storage/framework/cache/*

rm -rf public/client/app/**/*.js

rm -rf public/client/app/layout/menu.controller.js.bak

rm -rf public/client/app/samples

tar -cf deploy.tar server.php artisan app/ bootstrap/ config/ public/ resources/ storage/ vendor/ .env

gzip -9 deploy.tar

cp deploy.tar.gz ../

cd ../

rm -rf deploy

echo '\n:::: Pacote deploy.tar.gz gerado com sucesso! ::::\n'

exit
