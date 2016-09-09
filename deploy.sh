#!/bin/bash

mkdir deploy
chmod 777 deploy

cp -R /app/ /deploy
cp -R /bootstrap/ /deploy
cp -R /config /deploy
cp -R /public /deploy
cp -R /resources /deploy
cp -R /storage /deploy
cp -R /vendor /deploy
cp -R .env /deploy
cp -R artisan /deploy

cd deploy/

php artisan cache:clear
php artisan route:clear
php artisan view:clear
php artisan config:clear
php artisan config:cache
php artisan auth:clear-resets
php artisan clear-compiled

###gulp --production

rm -rf storage/logs/* storage/framework/sessions/* storage/framework/cache/*

rm -rf public/client/app/**/*.js

rm -rf public/client/app/samples

cd ../

tar -cf deploy.tar app/ bootstrap/ config/ public/ resources/ storage/ vendor/ .env

gzip -9 deploy.tar

##rm -rf deploy/

echo 'Pacote gerado com sucesso!'

exit
