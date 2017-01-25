## Pré requisitos ##

- Acesso livre ao PROXY.
- Preferencialmente utilize o Linux com o gerenciador APT.
  - Caso o SO seja windows utilize a instalação do projeto via Docker.
- Um editor decente.
    - Recomendado: [Visual Studio Code](https://code.visualstudio.com/) ou [ATOM](https://atom.io/).
- Git a versão mais recente [GIT](https://git-scm.com/book/pt-br/v1/Primeiros-passos-Instalando-Git).
- Permissão de leitura para todos os projetos do grupo Arquitetura no Git:
    - [Grupo Arquitetura](http://git.prodeb.ba.gov.br/groups/starter-pack).

## Requisitos da solução já fornecidos no Container Docker ##
- NodeJS versão 4 ou superior ([tutorial para instalar](https://nodejs.org/en/download/package-manager/)).
    - Configure o npm para rodar sem sudo ([tutorial](https://docs.npmjs.com/getting-started/fixing-npm-permissions)).
    - Verifique a versão do npm **npm --version** (deve ser igual ou superior a 3.5.1).
- PHP com a versão 5.6.25 ou superior ([tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)).
- Extensões do PHP: xdebug, fileinfo, mbstring, pdo_pgsql, pgsql, openssl.
- Composer ([tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)).
- PostgreSQL ([tutorial para instalar] (https://www.vivaolinux.com.br/dica/Instalando-o-PostgreSQL-e-pgAdmin3-no-Ubuntu)).

## Instalação ##

#### 1) Instalando e configurando o docker ####

> Caso o passo 1 já tenha sido realizado em outro momento pulo para o passo 2.

- Instale o docker [Docker Install](https://www.docker.com/products/overview).
- Instale o docker-compose [Docker Compose](https://docs.docker.com/compose/install/).
- No linux execute os comandos abaixo para criar o grupo do docker e adicionar o usuário.

```sh
sudo groupadd docker 
```

- em seguinda adicione o usuário ao grupo criado.

```sh
sudo usermod -aG docker $USER
```

- Realize o logoff para que as configurações do docker sejam aplicadas.

#### 2) Configurando o projeto ####

> Rode os comandos abaixo no terminal do linux:

```sh
git clone git@git.prodeb.ba.gov.br:starter-pack/laravel_angular_base.git {nome_projeto} 
cd {nome_projeto}
git clone git@git.prodeb.ba.gov.br:starter-pack/php-docker.git
cp .env.develoment .env
```
- O nome do host do postgres deve ser o nome do container postgres.

- Acesse o arquivo: **/php-docker/docker-compose.yml** e renomeie o container_name do DB e WEB
adicionando como sufixo o nome do projeto, ex:

```html
db:
  container_name: base-postgres-{nome_projeto}
web:
  container_name: base-php-fpm-{nome_projeto}
```

- Configure o .env com os dados de banco contidos no **/php-docker/docker-compose.yml**.
- Inicie o build e o container seguindo os passos abaixo.

```sh
cd {php-docker}
docker-compose build
docker-compose up
docker exec -it base-php-fpm-{nome_projeto} bash
chmod +x configure.sh
./configure.sh
```
 
-É aconselhável que se crie um alias no bash do host para executar comandos no bash do docker, para isso rode o comando abaixo:
    - **echo "alias {SEU_ALIAS}='docker exec -it base-php-fpm-{nome_projeto}'" >> ~/.bashrc**
    - (feche o console e abra novamente para que as alterações surtam efeito)
- Caso queira acessar a linha de comando do container rode **{ALIAS_CRIADO} bash**
- Todos os comandos no restante da documentação tem que ser prefixado com o container docker ou seu alias criado, de dentro da pasta do php-docker ex:
    - **docker exec -it base-php-fpm-{nome_projeto} {COMANDO}**
    - **{ALIAS_CRIADO} {COMANDO}**
- Para sair do bash digite **exit** e aperte enter

> Comandos úteis:

- Listar containers

```sh
docker ps
```

**Siga os próximos passos no README original**
**para mais informações e documentação acesse [Docker](https://www.docker.com/)**