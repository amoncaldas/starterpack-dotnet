npm install
bower install
chmod **777** -R storage
chmod **777** -R bootstrap/cache
composer install
php artisan key:generate
php artisan jwt:secret
php artisan migrate --seed