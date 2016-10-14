
# Sistema Starter Pack PHP #

> ## Iniciando

- [Sobre](#sobre)
- [Pré requisitos](#pre-requisitos)
- [Componentes](#componentes)

> ## Features

- [Instalação](#instalacao)
    - [Manual](#manual)
    - [Docker](#docker)
- [Colocar para Rodar](#colocar-para-rodar)
- [Desenvolvimento](#desenvolvimento)
    - [Editor](#editor)
    - [Gerador de Código](#gerador-de-codigo)
    - [Adicionar novo módulo angular](#adicionar-novo-modulo-angular)
    - [Configuração](#configuracao)
    - [Bibliotecas Externas](#bibliotecas-externas)
    - [Constantes](#constantes)
    - [Menu](#menu)
    - [Internacionalização](#internacionalizacao)
    - [Convenções](#convencoes)
    - [CRUD](#crud)
      - [Cliente](#no-client)
      - [Servidor](#no-server)
    - [Diretivas](#diretivas)
    - [Componentes NgProdeb](#componentes-ngprodeb)
    - [Ícones](#icons)
- [Log](#log)
- [Produção](#producao)

___
## Sobre ##

- Este projeto tem como objetivo servir de base para futuros projetos da Prodeb.
- O mesmo utiliza Laravel 5.1 com Angular 1.5.
- O sistema utiliza [JWT](http://jwt.io) para autenticação através da lib [tymon/jwt-auth](https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam um json contendo as informações requisitadas.

## Pré requisitos ##

- Preferencialmente utilize o Linux com o gerenciador APT.
- Um editor decente [vscode](https://code.visualstudio.com/) ou [atom.io](https://atom.io/).
- NodeJS versão 4 ou superior ([tutorial para instalar](https://nodejs.org/en/download/package-manager/)).
    - Configure o npm para rodar sem sudo ([tutorial](https://docs.npmjs.com/getting-started/fixing-npm-permissions)).
    - Verifique a versão do npm **npm --version** (deve ser igual ou superior a 3.5.1).
- PHP com a versão 5.6 ou superior ([tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)).
- Extenções do PHP: xdebug, fileinfo, mbstring, pdo_pgsql, pgsql, openssl.
- Composer ([tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)).
- Postgres e pgadmin3 ([tutorial para instalar] (https://www.vivaolinux.com.br/dica/Instalando-o-PostgreSQL-e-pgAdmin3-no-Ubuntu)).

## Componentes e Frameworks ##

> Componentes e frameworks utilizados no lado cliente do projeto:

- [AngularJS](https://angularjs.org)
- [Angular Material](https://material.angularjs.org)
- [NgProdeb](git@git.prodeb.ba.gov.br:ngprodeb.git)
- [momentjs](http://momentjs.com/)

## Instalação ##

> Você irá precisar de permissão para os repositórios Starter Pack PHP Angular, ngProdeb, php-docker e Generator NgProdeb.
> Rode os comandos abaixo no terminal do linux:

```sh
git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
cd {pasta_do_projeto}
cp .env.example .env
```

### Manual ###

#### Instalando os pré requisitos ####

> Instale todos os pré requisitos (php, nodejs, composer e etc...) antes de seguir
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

> rode os comandos abaixo para complentar a instalação:

```sh
source ~/.bashrc

sudo -u postgres psql
alter user postgres password 'root';
\q

sudo chown $(whoami):$(whoami) -R ~/.composer

npm install -g npm

echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p
```

> Ajuste o .env com as informações do banco de dados, email e etc...

#### Instalando o projeto ####

```sh
composer global require "laravel/installer=~1.1"
npm install -g yo gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower
sh configure.sh
```

### Docker ###

- instale o docker [Docker Install Linux](https://docs.docker.com/engine/installation/linux/)
- instale o docker-compose [Docker Compose](https://docs.docker.com/compose/install/)
- realize o logoff para que as configurações do docker sejam aplicadas
- configure o .env com os dados contidos no **/php-docker/docker-compose.yml** 
- o nome do host do postgres deve ser o nome do container postgres

```sh
git clone git@git.prodeb.ba.gov.br:php-docker.git
cd php-docker
docker-compose build
docker-compose up
docker exec -it base-php-fpm bash
chmod +x configure.sh
./configure.sh
```

- é aconselhável que se crie um alias no bash do host para executar comandos no bash do docker, para isso rode o comando abaixo:
    - **echo "alias {SEU_ALIAS}='docker exec -it base-php-fpm'" >> ~/.bashrc**
    - (feche o console e abra novamente para que as alterações surtam efeito)
- caso queira acessar a linha de comando do container rode **{ALIAS_CRIADO} bash**
- todos os comandos no restante da documentação tem que ser prefixado com o container docker ou seu alias criado, de dentro da pasta do php-docker ex:
    - **docker exec -it base-php-fpm {COMANDO}**
    - **{ALIAS_CRIADO} {COMANDO}**
- para sair do bash digite **exit** e aperte enter

## Colocar para Rodar ##

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente

```sh
cd pasta_do_projeto
gulp
```

- parametros opcionais:
  - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )

> Em outra aba do terminal rode o comando abaixo para levantar o servidor php:

```sh
npm run server (Este comando inicia o servidor php na porta 5000)
```

> Abra o navegador

- acesse **http://localhost:5000**
- logue com os dados, email: **admin-base@prodeb.com** e senha: **Prodeb01**

> Outros comandos

```sh
gulp check
```

- (verifica a formatação do código javascript)
  - parametros opcionais
    - **--fix** (para corrigir os erros que podem ser corrigidos automaticamente)

## Desenvolvimento ##

> ### Editor ###

- [vscode](https://code.visualstudio.com/)
  - plugins utilizados:
      - php debug
      - php code format
      - eslint (para verificar erros de formatação e code smell)
      - editor config (para configurar codificação, tabulação ...)
      - beautify (para formatar o código)
      - vscode-icons
      - path intellisense (autocomplete para php)
      - angular material snippets
      - auto close tag

> ### Geradores automático de arquivos ###

- Use os geradores de estrutura de arquivo para gerar os arquivos necessários para o recurso,

>  Para gerar arquivos de estrutura do lado do cliente, use o comando abaixo:

```sh
cd {pasta_do_projeto}
yo ngprodeb
```

- escolha a estrutura na lista
- digite o nome do recurso

**para mais detalhes sobre o uso do gerador acesse [Generator NgProdeb](http://git.prodeb.ba.gov.br/generator-ngprodeb/tree/master)**

> Para gerar arquivos de estrutura do lado do servidor, use os comandos abaixo:

- Estrutura completa

```sh
php artisan crud:generate Posts --fields="{field_1}#string; {field_2}#text;" --controller-namespace={Recurso} --route={recurso}
```

- Controller

```sh
php artisan crud:controller {Recurso}Controller --crud-name={recurso} --model-name={Recurso} --route={recurso}
```

- Model

```sh
php artisan crud:model {Recurso} --fillable="['field_1', 'field_2']"
```

- Migration

```sh
php artisan crud:migration {recurso} --schema="field_1#string; field_2#text"
```

> Após criado todos os recursos, rode o comando abaixo para aplicar as migrations criadas
> se necessário, inclua o caminho no arquivo **/app/Http/routes.php**

```sh
php artisan migrate
```

**para mais detalhes sobre o uso do gerador acesse [CRUD Generator](https://github.com/appzcoder/crud-generator#commands)**

> ### Adicionar novo módulo angular ###

- adicione a dependência no arquivo bower.json
- rode o comando

```sh
bower install {nome-da-biblioteca}
```

- adicione o caminho da dependência no arquivo gulpfile.js
  - para importação angular adicione no array **paths.angularScripts**
  - ao adicionar um novo módulo o gulp deve ser reiniciado
- adicione o módulo no arquivo public/client/app/app.js

> ### Configuração ###

- acesse o arquivo /public/client/app/app.config.js
- $translateProvider
  - configura o módulo de tradução das strings
- moment.locale('');
  - configura o idioma das datas
- $mdThemingProvider
  - configura o tema do angular material

> ### Bibliotecas Externas ###
> (bibliotecas que não são módulos do angular)

- acesse o arquivo **public/client/app/app.external.js**
- adicione a linha:

```javascript
.constant('{NOME_DA_CONSTANTE}', {NOME_BIBLIOTECA});
```

> ### Constantes ###

- acesse o arquivo **public/client/app/app.global.js**
- adicione um novo atributo contendo o nome da constante e o seu valor

> ### Menu ###
(adicionando itens ao menu)

- acesse o arquivo **public/client/app/layout/menu.controller.js**
- adicione um objeto no array **vm.itensMenu**

> exemplo de um item no menu:

```javascript
{
  url: '{STATE}',
  titulo: menuPrefix + '{CHAVE_ARQUIVO_LANGUAGE}',
  icon: '{MATERIAL_ICON}',
  subItens: []
}
```

> exemplo de um item no menu com sub itens:<br>

```javascript
{
  url: '#',
  titulo: menuPrefix + '{CHAVE_ARQUIVO_LANGUAGE}',
  icon: '{MATERIAL_ICON}',
  profiles: ['{PERFIL}'],
  subItens: [
    {
      url: '{STATE}',
      titulo: menuPrefix + '{CHAVE_ARQUIVO_LANGUAGE}',
      icon: '{MATERIAL_ICON}'
    }
  ]
}
```

> ### Internacionalização ###

  - todas as strings usadas no sistema devem ser armazenadas no objeto data localizado no arquivo **public/client/app/i18n/language-loader.service.js**
  - estrutura do arquivo:
      - no primeiro momento estão as strings comuns ao sistema como um todo
      - em seguida as strings das views subdivididas em blocos
          - strings dos breadcrumbs
          - strings dos titles
          - strings das actions
          - strings dos fields
          - strings do layout
          - string dos tooltips
      - strings dos atributos dos recursos
      - strings dos dialogs
      - strings das mensagens
      - por fim as strings com os nomes dos models(recurso)
  - por convenção o padrão utilizado é o seguinte:
      - bloco das strings comuns ao todo
      - blocos das strings específicas
          - blocos das strings comuns específicas
          - blocos das strings por recurso
  
> ### Convenções ###
> (convenções adotadas para padronização do projeto)

  - o conjunto de arquivos são chamados de recurso(resource) localizados sempre no caminho **public/client/app**
  - cada recurso pode pussuir os seguintes arquivos:
    - recursos.html(index)
    - recursos-list.html
    - recursos-form.html
    - recursos.controller.js
    - recursos.route.js
    - recursos.service.js
  - deve ser usado o gerador de estrutura de arquivos para gerar os arquivos no padrão informado acima
  - no lado servidor ao ser criado o controller deve-se mudar a herança de Controller para **CrudController**
  o mesmo acontece quando um model é criado deve-ser mudar a herança de Model para **BaseModel**
  - as imagens devem ser armazenadas no caminho **public/client/images**
  - para alterar as propriedades de css acesse o arquivo **public/client/styles/app.scss**
  - os templates dos emails devem ser salvos no caminho **resources/views/mails**

> ### CRUD ###

- Existe 2 controllers base contendo todas as ações padrões de um CRUD, são eles:

#### No Client ####

**crud.controller.js** (public/client/app/core/crud.controller.js)

- Para herdar as funciolidades basta, no controller executar:

```javascript
$controller('CRUDController', 
  { 
    vm: vm, 
    modelService: {MODEL_SERVICE}, 
    options: { } 
  }
);
```

- Opções

```javascript
{
  redirectAfterSave: {BOOLEAN},
  searchOnInit: {BOOLEAN},
  perPage: {QUANTIDADE_POR_PAGINA}
}
```

- Ações Implementadas

```javascript
activate()
search(page)
edit(resource)
save()
remove(resource)
goTo(viewName)
cleanForm()
```

- Gatilhos

```javascript
onActivate()
applyFilters(defaultQueryFilters)//recebe um objeto com os filtros de página aplicado e deve devolver este objeto com demais filtros
beforeSearch(page) //retornando false cancela o fluxo
afterSearch(response)
beforeClean //retornando false cancela o fluxo
afterClean()
beforeSave() //retornando false cancela o fluxo
afterSave(resource)
beforeRemove(resource) //retornando false cancela o fluxo
afterRemove(resource)
```

- Exemplo

```javascript

angular
  .module('app')
  .controller('{NOME_DO_CONTROLLER}', {NOME_DO_CONTROLLER});

function {NOME_DO_CONTROLLER}($controller, {MODEL_SERVICE}) {
  var vm = this;

  vm.onActivate = onActivate;
  vm.applyFilters = applyFilters;

  $controller('CRUDController', { vm: vm, modelService: {MODEL_SERVICE}, options: {} });

  function onActivate() {
    vm.models = {MODEL_SERVICE}.listModels();
    vm.types = {MODEL_SERVICE}.listTypes();

    vm.queryFilters = { type: vm.types[0].id, model: vm.models[0].id };
  }

  function applyFilters(defaultQueryFilters) {
    return angular.extend(defaultQueryFilters, vm.queryFilters);
  }

}
```

#### No Server ####

**CrudController.php** (app/Http/controllers/CrudController.php)

- Para herdar as funciolidades basta, no controller executar:

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
        $query = $query->with('{relationships}');

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

Obs: Exceto para as datas que já são pré formatadas, podendo ocorrer erros caso o padrão seja modificado

> ### Diretivas ###

O uso de todos os componentes são demonstrados através das funcionalidades de exemplo adiconadas na pasta **public/client/app/samples**

- __ContentHeader__

```html
<content-header title="" description="">
  Conteúdo do content header
</content-header>
```

- __ContentBody__

```html
<content-body layoutAlign="">
  Conteúdo do content header.
</content-body>
```

- __Box__
(obs.: o box deve estar dentro de um ContentBody)

```html
<box box-title="{Título do box}">
  Conteúdo do box
</box>
```

```html
<box box-title="{Título do box}">
  <box-toolbar-buttons>
    Botões no toolbar do box (Opcional)
  </box-toolbar-buttons>
    Conteúdo do box
  <box-footer-buttons>
    Botões no rodapé do box (Opcional)
  </box-footer-buttons>
</box>
```

- ( para mais exemplos consulte **public/client/app/samples** )

> ### Componentes NgProdeb ###

- PrPagination
- PrSpinner
- PrDataPicker
- CKEditor
- PrDialog
- PrToast

**Para saber como usar os componentes acesse: [Git NgProdeb](http://git.prodeb.ba.gov.br/ngprodeb)**

> ### Ícones ###

- Os icones usados no sistema são encontrados em [Material Icons](https://design.google.com/icons/) e seguem o padrão abaixo:

```html
<md-icon md-font-set="material-icons">
  {3d_rotation}
</md-icon>
```

## Log ##

> Para ver os logs

- acesse [http://localhost:5000/developer/log-viewer](http://localhost:5000/developer/log-viewer)
- digite o usuário conforme a variável de ambiente no arquivo .env DEVELOP_ID
- digite a senha conforme a variável de ambiente no arquivo .env DEVELOP_PASSWORD

## Produção ##

- altere os dados do arquivo .env.production com as configurações de produção (banco, smtp, nível de log, ftp e etc) e desative o debug.
- rode o comando **npm run package** 
    - prepara a aplicação para produção
    - minificando os arquivos js, css e modificando o index.html para apontar para os arquivos minificados
    - gerando o pacote zipado no padrão **{NomeProjeto}.tar.gz**
- o sistema irá perguntar se deseja enviar para o ftp, caso queria o pacote será enviado e removido da raiz do projeto
- caso contrário o arquivo **{NomeProjeto}.tar.gz** constará na raiz do projeto para o devido uso