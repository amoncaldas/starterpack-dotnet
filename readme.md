
# Sistema Starter Pack PHP #

> ## Iniciando

- [Sobre](#sobre)
- [Pré requisitos](#pre-requisitos)
- [Componentes e Frameworks](#componentes-e-frameworks)

> ## Features

- [Funcionalidades](#funcionalidades)
- [Instalação](#instalacao)
    - [Manual](#manual-passo-a-passo)
    - [Docker](#docker-passo-a-passo)
- [Colocando para Rodar](#colocar-para-rodar)
- [Desenvolvimento](#desenvolvimento)
    - [Editor](#editor)
    - [Geradores automáticos de arquivos](#geradores-automaticos-de-arquivos)
    - [Convenções](#convencoes)
    - [CRUD](#crud)
    - [Formatação de atributos](#formatacao-de-atributos)
- [Log](#log)
- [Produção](#producao)

___
## Sobre ##

- Este projeto tem como objetivo servir de base para futuros projetos da PRODEB.
- Como framework backend o sistema utiliza o Laravel 5.1.
- Como framework frontend o sistema utiliza o AngularJS 1.5.
- Para autenticação o sistema utiliza [JWT](http://jwt.io) através da lib [tymon/jwt-auth](https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam um json contendo as informações requisitadas.

## Pré requisitos ##

- Acesso livre ao PROXY.
- Preferencialmente utilize o Linux com o gerenciador APT.
  - Caso o SO seja windows utilize a instalação do projeto via Docker.
- Um editor decente.
    - Recomendado: [Visual Studio Code](https://code.visualstudio.com/) ou [ATOM](https://atom.io/).
- NodeJS versão 4 ou superior ([tutorial para instalar](https://nodejs.org/en/download/package-manager/)).
    - Configure o npm para rodar sem sudo ([tutorial](https://docs.npmjs.com/getting-started/fixing-npm-permissions)).
    - Verifique a versão do npm **npm --version** (deve ser igual ou superior a 3.5.1).
- Git a versão mais recente [GIT](https://git-scm.com/book/pt-br/v1/Primeiros-passos-Instalando-Git).
- PHP com a versão 5.6.25 ou superior ([tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)).
- Extensões do PHP: xdebug, fileinfo, mbstring, pdo_pgsql, pgsql, openssl.
- Composer ([tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)).
- PostgreSQL e pgAdmin ([tutorial para instalar] (https://www.vivaolinux.com.br/dica/Instalando-o-PostgreSQL-e-pgAdmin3-no-Ubuntu)).
- Permissão de leitura para todos os projetos do grupo Arquitetura no Git:
    - [Grupo Arquitetura](http://git.prodeb.ba.gov.br/groups/starter-pack).

## Componentes e Frameworks ##

> Componentes e frameworks utilizados no frontend do projeto:

- [AngularJS](https://angularjs.org)
- [Angular Material](https://material.angularjs.org)
- [NgProdeb](https://git.prodeb.ba.gov.br:starter-pack/ngprodeb)
- [MomentJS](http://momentjs.com/)

## Funcionalidades ##

> Funcionalidades atualmente disponíveis no projeto

- Instalação via Docker - [PHP Docker](http://git.prodeb.ba.gov.br/starter-pack/php-docker);
- Autenticação via token com o JWT (Dispensando o uso de sessão);
- Gerador automático de estrutura de arquivo (Servidor);
- Encapsulamento do CRUD no servidor com a classe **CrudController**;
- Formatação dos atributos do Model no servidor;
- LOG (Gerenciador de logs do sistema);
- Auditoria;
- Gerador de pacote para produção;
- Envio do pacote via FTP;
- Implementação do [CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/Access_control_CORS);
- Gerenciamento de usuário;
- Consultas dinâmicas;
- Envio de emails para usuário;
- Integração com o [Angular Client](http://git.prodeb.ba.gov.br/starter-pack/starter-pack-angular-client.git);
- Integração com o [Generator NgProdeb](http://git.prodeb.ba.gov.br/starter-pack/generator-ngprodeb.git);

## Instalação ##

> Rode os comandos abaixo no terminal do linux:

```sh
git clone --recursive git@git.prodeb.ba.gov.br:starter-pack/laravel_angular_base.git {nome_projeto} 
cd {nome_projeto}
cp .env.develoment .env
```

> Ajuste o .env com as informações do banco de dados, email e etc...

### Manual Passo-a-Passo ###

#### 1) Instalando os pré requisitos ####

**Obs.: Caso os pré requisitos já estejam instalados passe para o passo 3**

> Em uma instalação limpa do Linux Mint ou Ubuntu os comandos a seguir instalam os pré requisitos:

```sh
curl -sL https://deb.nodesource.com/setup_4.x | sudo bash -

sudo apt-get update && sudo apt-get install -y build-essential libxml2-dev libfreetype6-dev libjpeg-turbo8-dev libmcrypt-dev libpng12-dev libssl-dev libpq-dev git vim unzip postgresql-9.5 postgresql-client nodejs php7.0 php7.0-pgsql php7.0-xml php7.0-zip php7.0-cli php7.0-common php7.0-gd php7.0-mbstring php7.0-mcrypt php7.0-readline php7.0-json pgadmin3

sudo curl -sS https://getcomposer.org/installer | sudo php -- --install-dir=/usr/local/bin --filename=composer

mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
```

> adicione a linha a seguir no final do arquivo **~/.bashrc**.

```sh
export PATH=~/.npm-global/bin:$PATH
```

> rode os comandos abaixo para completar a instalação:

```sh
source ~/.bashrc

sudo -u postgres psql
alter user postgres password 'root';
\q

sudo chown $(whoami):$(whoami) -R ~/.composer

npm install -g npm
```

#### 2) Aplicando fix para alterar limite de watches do gulp ####

```sh
echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p
```

#### 3) Instalando e configurando o projeto ####

> Linux

```sh
cd {nome_projeto}
sh configure.sh
```

> Windows

- Execute o arquivo configure.bat que está na pasta raiz do projeto

### Docker Passo-a-Passo ###

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

- O nome do host do postgres deve ser o nome do container postgres.
- Faça o clone do projeto:

```sh
git clone git@git.prodeb.ba.gov.br:starter-pack/php-docker.git
```

- Acesse o arquivo: **/php-docker/docker-compose.yml** e renomeie o container_name do DB e WEB
adicionando como sufixo o nome do projeto, ex:

```html
db:
  container_name: base-postgres-{nome_projeto}
web:
  container_name: base-php-fpm-{nome_projeto}
```

- Configure o .env com os dados contidos no **/php-docker/docker-compose.yml** ou com os dados especifícos do banco.
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

**para mais informações e documentação acesse [Docker](https://www.docker.com/)**

## Colocando para Rodar ##

> Caso esteja clonando para dar inicio a um novo projeto rode o comando abaixo. 
> Se for contribuir com o Starter Pack pule o próximo comando

```sh
cd {pasta_do_projeto}
rm -rf .git public/client/.git .gitmodules 
```

> Ajuste o public/client/paths.json com o path relativo (disco) da pasta client clonada na área public do servidor: ex: **"serverClientPath": "client"**
> Ajuste o public/client/app/app.global.js com as informações de paths (servidor) do client, images e api, ex:

```javascript
clientPath: 'client/app',
apiPath: 'v1',
imagePath: 'client/images'
```

> Após a configuração, remova o paths.json e o app.global.js do arquivo **public/client/.gitignore** 
> e execute o comando abaixo:

```sh
rm -rf public/client/app/app.global.js.example public/client/paths.json.example
```

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente

```sh
cd {pasta_do_projeto}
npm run build
```

- Parametros opcionais:
  - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )

> Em outra aba do terminal rode o comando abaixo para levantar o servidor php:

```sh
cd {pasta_do_projeto}
npm run server (Este comando inicia o servidor php na porta 5000)
```

> Abra o navegador

- Acesse **http://localhost:5000**
- Logue com os dados, email: **admin-base@prodeb.com** e senha: **Prodeb01**

> Outros comandos

```sh
npm run check
```

- Verifica a formatação do código javascript
  - parametros opcionais
    - **--fix** (para corrigir os erros que podem ser corrigidos automaticamente)

```sh
npm run package
```

- Prepara o projeto para o ambiente informado e envia para o FTP

## Desenvolvimento ##

> ### Editor ###

- [vscode](https://code.visualstudio.com/)
  - plugins utilizados:
      - php debug
      - php code format
      - eslint (para verificar erros de formatação e code smell no javascript)
      - editor config (para configurar codificação, tabulação ...)
      - beautify (para formatar o código)
      - path intellisense (autocomplete para php)
      - angular material snippets
      - auto close tag
      - html css class completion

> ### Geradores automáticos de arquivos ###

- Use os geradores de estrutura de arquivo para gerar os arquivos necessários para o recurso.

>  Para gerar estrutura de arquivos do lado cliente, use o comando abaixo:

```sh
cd {pasta_do_projeto}
yo ngprodeb
```

- Escolha a estrutura na lista
- Digite o nome do recurso

**Para mais detalhes sobre o uso do gerador acesse [Generator NgProdeb](http://git.prodeb.ba.gov.br/generator-ngprodeb/tree/master)**

> Para gerar estrutura de arquivos do lado servidor, use os comandos abaixo:

- Estrutura completa

```sh
php artisan crud:generate {Recurso} --fields="{field_1}#string; {field_2}#text;" --controller-namespace={Recurso} --route-group={groupName}
```

- Controller

```sh
php artisan crud:controller {Recurso}Controller --crud-name={recurso} --model-name={Recurso} --route-group={recurso}
```

- Model

```sh
php artisan crud:model {Recurso} --fillable="['{field_1}', '{field_2}']"
```

- Migration

```sh
php artisan crud:migration {recurso} --schema="{field_1}#string; {field_2}#text"
```

> Obs.: Após a criação da Estrutura completa ou de uma Migration acesse o arquivo de migration
> dentro da pasta database > migrations e Remova a linha **$table->timestamps()** e adicione as linhas abaixo:

```php
$table->timestampTz('created_at');
$table->timestampTz('updated_at');
```

> Após o processo, rode o comando abaixo para aplicar as migrations criadas

```sh
php artisan migrate
```

> Se necessário, inclua uma nova rota no arquivo **/app/Http/routes.php**

**Para mais detalhes sobre o uso do gerador acesse [CRUD Generator](https://github.com/appzcoder/crud-generator#commands)**
  
> ### Convenções ###
> (convenções adotadas para padronização do projeto)

  - Deve ser usado o gerador de estrutura de arquivos para gerar os arquivos no padrão que o sistema comporta

> ### CRUD ###

**CrudController.php** (app/Http/controllers/CrudController.php)

- Para herdar as funcionalidades, basta, no controller executar:

```php
use App\Http\Controllers\CrudController;

class {NOME_DO_CONTROLLER} extends CrudController
```

- Deve ser implementado os métodos

```php
getModel() //retornar a classe referente ao model
getValidationRules(Request $request, Model $obj) //retornar um array com as regras de validação
```

- Ações Implementadas

```php
index(Request $request)
store(Request $request)
show(Request $request, $id)
update(Request $request, $id)
saveOrUpdate(Request $request, $obj, $action)
destroy(Request $request, $id)
```

- Gatilhos

```php
applyFilters(page, $request, $baseQuery)
beforeAll($request)
beforeSearch($request, $dataQuery, $countQuery)
beforeSave($request, $obj)
beforeStore($request, $obj)
beforeUpdate($request, $obj)
beforeDestroy($request, $obj)
afterSave($request, $obj)
afterStore($request, $obj)
afterUpdate($request, $obj)
afterDestroy($request, $obj)
```

- Exemplo

```php
class ProjectsController extends CrudController
{
    public function __construct()
    {
    }

    protected function getModel()
    {
        return {MODEL}::class;
    }

    protected function applyFilters(Request $request, $query) {
        $query = $query->with('{relacionamento}');

        if($request->has('name'))
            $query = $query->where('name', 'like', '%'.$request->name.'%');
    }

    protected function beforeSearch(Request $request, $dataQuery, $countQuery) {
        $dataQuery->orderBy('name', 'asc');
    }

    protected function getValidationRules(Request $request, Model $obj)
    {
        $rules = [
            'name' => 'required|max:100|unique:{resource}'
        ];

        if ( strpos($request->route()->getName(), '{resource}.update') !== false ) {
            $rules['name'] = 'required|max:255|unique:{resource},name,'.$obj->id;
        }

        return $rules;
    }
}
```

> ### Formatação de atributos ###

Para formatar os atributos no lado do servidor, deve ser adicionado no array de cast no construtor do model
como no exemplo abaixo:

```php
public function __construct($attributes = array())
{
    parent::__construct($attributes);

    $this->addCast(['{atributo}' => '{formato}']);
}
```

Obs: Exceto para as datas que já são pré formatadas, podendo ocorrer erros caso o padrão seja modificado.

> ### Para fazer uma validação específica ###

Em qualquer action de um CrudController é possível adicionar validações específicas com o mesmo padrão de resposta esperado. 

```php
  $this->validate($request, []);

  if($objeto->owner_id !== Auth::id()) {
      $this->validator->errors()->add('owner', 'Este recurso não é seu');
      $this->throwValidationException($request, $this->validator);
  }
```

## Log ##

> Para ver os logs

- Acesse [http://localhost:5000/developer/log-viewer](http://localhost:5000/developer/log-viewer)
- Digite o usuário conforme a variável de ambiente no arquivo .env DEVELOP_ID
- Digite a senha conforme a variável de ambiente no arquivo .env DEVELOP_PASSWORD

## Produção ##

> Siga os passos abaixo para gerar o pacote para produção:

- Altere os dados do arquivo .env.production com as configurações de produção (pkg_name, banco, smtp, nível de log, ftp e etc) e desative o debug.
- Rode o comando **npm run package**.

> Prepara a aplicação para produção minificando os arquivos js, css e modificando o index.html para apontar para os arquivos minificados
> gerando o pacote zipado no padrão **{NomeProjeto}.zip**.

- Em seguida o sistema irá perguntar se deseja enviar para o ftp, caso queira, o pacote será enviado 
descompactado e removido da raiz local do projeto e do ftp logo após abrindo o navegador padrão no endereço informado no APP_URL no arquivo .env,
caso contrário, o arquivo **{NomeProjeto}.zip** constará na raiz do projeto para o devido uso.