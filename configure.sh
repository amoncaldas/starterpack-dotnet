git submodule update --init --recursive

composer global require "laravel/installer=~1.1"

# Instalando o Yoman
npm install -g yo

# Instalando as dependencias locais gulp, eslint, bower e derivados
npm install gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower

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

# Dando permissão de execussão no arquivo de deploy
chmod +x deploying.sh

# Instalando as dependencias do frontend
cd public/client
npm install
chown -R $(whoami):$(whoami) /home/$(whoami)/.config/configstore/
bower install

# Acessando o root
cd ../..

# Renomenado arquivos
cp public/client/paths.json.example public/client/paths.json
cp public/client/app/app.global.js.example public/client/app/app.global.js
