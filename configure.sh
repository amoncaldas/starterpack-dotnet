npm install -g git+ssh://git@git.prodeb.ba.gov.br:generator-ngprodeb.git
npm install
bower install
chmod 777 -R storage
chmod 777 -R bootstrap/cache
COMPOSER_PROCESS_TIMEOUT=2000 composer install
php artisan key:generate
php artisan jwt:secret
php artisan migrate --seed
chmod +x deploying.sh
