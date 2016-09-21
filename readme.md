
# Sistema Base Laravel #

> Iniciando
- [Sobre](#sobre)
- [Pré requisitos](#pre_requisitos)
- [Componentes](#componentes)

> Features
- [Instalação](#instalacao)
  - [Manual](#manual)
  - [Docker](#docker)
- [Colocar para Rodar](#rodando)
- [Desenvolvimento](#desenvolvimento)
  - [Editor](#editor)
  - [Gerador de Código](#gerador)
  - [Adicionar novo módulo angular](#novo_modulo)
  - [Configuração](#configuracao)
  - [Bibliotecas Externas](#bibliotecas)
  - [Constantes](#constantes)
  - [Menu](#menu)
  - [Internacionalização](#language)
  - [Convenções](#convencoes)
  - [CRUD](#crud)
    - [Cliente](#crud_cliente)
    - [Servidor](#crud_servidor)
  - [Diretivas](#diretivas)
  - [Componentes NgProdeb](#componentesNgProdeb)
  - [Ícones](#icons)
- [Log](#log)
- [Produção](#producao)

___
## <a name="sobre"></a> Sobre

- Este projeto tem como objetivo servir de base para futuros projetos da Prodeb.
- O mesmo utiliza Laravel 5.1 com Angular 1.5.
- O sistema utiliza [JWT](http://jwt.io) para autenticação através da lib [tymon/jwt-auth](https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam json contendo as informações da requisitadas.

## <a name="pre_requisitos"></a> Pré requisitos ##

- Preferencialmente utilize o Linux com o gerenciador APT
- editor decente [vscode](https://code.visualstudio.com/) ou [atom.io](https://atom.io/)
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
    - Configure o npm para rodar sem sudo [tutorial](https://docs.npmjs.com/getting-started/fixing-npm-permissions)
- php versão 5.6 ou superior [tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)
- extenções do php: xdebug, fileinfo, mbstring, pdo_pgsql, pgsql, openssl
- composer [tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)
- postgres

## <a name="componentes"></a> Componentes ##

> Componentes e frameworks utilizados no projeto

- [AngularJS](https://angularjs.org)
- [Angular Material](https://material.angularjs.org)
- [NgProdeb](git@git.prodeb.ba.gov.br:ngprodeb.git)
- [momentjs](http://momentjs.com/)

## <a name="instalacao"></a> Instalação ##

> Rode os comandos abaixo.
> Todos os comandos devem ser executados no terminal do linux.

```sh
git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
cd pasta_do_projeto
cp .env.example .env
```

> Siga a instalação manual ou com o Docker

### <a name="manual"></a> Manual ###

> Instale todos os pre requisitos (php, node ...) antes de seguir
> Ajuste o .env com os dados do banco, email ...

```sh
composer global require "laravel/installer=~1.1"
npm install -g yo gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower git+ssh://git@git.prodeb.ba.gov.br:generator-ngprodeb.git
sh configure.sh
```

> Configure o .env com os dados da conexão do postgres

### <a name="docker"></a> Docker ###

> instale o docker 
> realize o logoff para que as configurações do docker sejam aplicadas
> configure o .env com os dados contidos no **docker-compose.yml**
> * o nome do host do postgres deve ser o nome do container postgres

```sh
git clone git@git.prodeb.ba.gov.br:php-docker.git
cd php-docker
docker-compose build
docker-compose up
docker exec -it base-php-fpm chmod +x configure.sh
docker exec -it base-php-fpm sh configure.sh
```

> caso queira acessar a linha de comando do container rode docker exec -it base-php-fpm bash
> todos os comandos no restante da documentação tem que ser prefixado com o container docker, de dentro da pasta do php-docker ex:
> docker exec -it base-php-fpm gulp , docker exec -it base-php-fpm bower installl ...

## <a name="rodando"></a> Colocar para Rodar ##

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente

```sh
cd pasta_do_projeto
gulp
```

  - parametros opcionais
    - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )

 - Caso dê erro sobre quantidade de arquivos observados no linux, execute o comandos no terminal
    - echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p


> Em outra aba do terminal.

```sh
npm run server (Este comando inicia o servidor php na porta 5000)
```

> Abra o navegador

- acesse **http://localhost:5000**
- logue com os dados email: **admin-base@prodeb.com** senha: **Prodeb01**

> Outros comandos

```sh
gulp check
```
- (verifica a formatação do código javascript)
  - parametros opcionais
    - **--fix** (para corrigir os erros que podem ser corrigidos automaticamente)

## <a name="desenvolvimento"></a> Desenvolvimento ##

> ### <a name="editor"></a> Editor ###

- [vscode](https://code.visualstudio.com/)
  - plugins utilizados:
    - php debug
    - php code format
    - eslint (para verificar erros de formatação e code smell)
    - editor config (para configurar codificação, tabulação ...)
    - beautify (para formatar o código)
    - vscode-icons
    - path intellisense (autocomplete para php)

> ### <a name="gerador"></a> Gerador de Código ###

- Use o gerador de estrutura de arquivo para gerar os arquivos necessários para o recurso,
- foi usado o [YEOMAN](http://yeoman.io) para criar o gerador.

```sh
cd pasta_do_projeto
yo ngprodeb
```
- escolha a estrutura na lista
- digite o nome do recurso
- para mais detalhes sobre o uso do gerador acesse o repositório do mesmo [Generator NgProdeb](http://git.prodeb.ba.gov.br/generator-ngprodeb/tree/master)

> ### <a name="novo_modulo"></a> Adicionar novo módulo angular ###

- adicione a dependência no arquivo bower.json
- rode o comando

```sh
bower install nome-da-biblioteca
```
- adicione o caminho da dependência no arquivo gulpfile.js
  - para importação angular adicione no array **paths.angularScripts**
  - ao adicionar um novo módulo o gulp deve ser reiniciado
- adicione o módulo no arquivo public/client/app/app.js

> ### <a name="configuracao"></a> Configuração ###

- acesse o arquivo /public/client/app/app.config.js
- $translateProvider
  - configura o módulo de tradução das strings
- moment.locale('');
  - configura o idioma das datas
- $mdThemingProvider
  - configura o tema do angular material

> ### <a name="bibliotecas"></a> Bibliotecas Externas ###
> (bibliotecas que não são módulos do angular)

- acesse o arquivo **public/client/app/app.external.js**
- adicione a linha:

```javascript
.constant('NOME_DA_CONSTANTE', NOME_BIBLIOTECA);

```

> ### <a name="constantes"></a> Constantes ###

- acesse o arquivo **public/client/app/app.global.js**
- adicione um novo atributo contendo o nome da constante e o seu valor

> ### <a name="menu"></a> Menu (adicionando itens ao menu) ###

- acesse o arquivo **public/client/app/layout/menu.controller.js**
- adicione um objeto no array **vm.itensMenu**
  > exemplo de um item no menu:

  ```javascript
  {
    url: 'dashboard',
    titulo: menuPrefix + 'dashboard',
    icon: 'icon_material_design',
    subItens: []
  }

  ```

  > exemplo de um item no menu com sub itens:<br>

  ```javascript
  {
    url: '#',
    titulo: menuPrefix + 'admin',
    icon: 'icon_material_design',
    profiles: ['admin'],
    subItens: [
      {
        url: 'user',
        titulo: menuPrefix + 'user',
        icon: 'icon_material_design'
      }
    ]
  }
  ```
  > os icones usados no sistema encontram-se em [Material Icons](https://design.google.com/icons/)

> ### <a name="language"></a> Internacionalização ###

  - todas as strings usadas no sistema devem ser armazenadas no objeto data localizado no arquivo **public/client/app/i18n/language-loader.service.js**
  - estrutura do arquivo:
    - no primeiro momento estão as strings comuns ao sistema como um todo
    - em seguida as strings comuns aos formulários
    - strings dos dialogs
    - strings das mensagens do toast
    - strings dos breadcrumbs
    - strings comuns a todos os models(recurso)
    - strings comuns aos controllers
    - por fim as strings comuns a cada recurso

> ### <a name="convencoes"></a> Convenções ###
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

> ### <a name="crud"></a> CRUD ###

- Existe 2 controllers base contendo todas as ações padrões de um CRUD, são eles:
- <a name="crud_cliente"></a> No client - **crud.controller.js** (public/client/app/core/crud.controller.js)
  - Para herdar as funciolidades basta, no controller executar:

    ```javascript
    $controller('CRUDController', { vm: vm, modelService: ProjectsService, options: { } });
    ```

  - Opções

    ```javascript
    {
      redirectAfterSave: true,
      searchOnInit: true,
      perPage: 8
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
    applyFilters(defaultQueryFilters)
      //recebe um objeto com os filtros de página aplicado e deve devolver este objeto com demais filtros
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
      .controller('AuditController', AuditController);

    function AuditController($controller, AuditService, PrDialog) {
      var vm = this;

      vm.onActivate = onActivate;
      vm.applyFilters = applyFilters;

      $controller('CRUDController', { vm: vm, modelService: AuditService, options: {} });

      function onActivate() {
        vm.models = AuditService.listModels();
        vm.types = AuditService.listTypes();

        vm.queryFilters = { type: vm.types[0].id, model: vm.models[0].id };
      }

      function applyFilters(defaultQueryFilters) {
        return angular.extend(defaultQueryFilters, vm.queryFilters);
      }

    }
    ```

- <a name="crud_servidor"></a> No Server - **CrudController.php** (app/Http/controllers/CrudController.php)

  - Para herdar as funciolidades basta, no controller executar:

    ```php
    use App\Http\Controllers\CrudController;

    class ProjectsController extends CrudController
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
            return Project::class;
        }

        protected function applyFilters(Request $request, $query) {
            $query = $query->with('tasks');

            if($request->has('name'))
                $query = $query->where('name', 'like', '%'.$request->name.'%');
        }

        protected function beforeSearch(Request $request, $dataQuery, $countQuery) {
            $dataQuery->orderBy('name', 'asc');
        }

        protected function getValidationRules(Request $request, Model $obj)
        {
            $rules = [
                'name' => 'required|max:100|unique:projects',
                'cost' => 'required|min:1'
            ];

            if ( strpos($request->route()->getName(), 'projects.update') !== false ) {
                $rules['name'] = 'required|max:255|unique:projects,name,'.$obj->id;
            }

            return $rules;
        }
    }
    ```

> ### <a name="diretivas"></a> Diretivas ###

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
  <box box-title="Título do box">
    Conteúdo do box
  </box>
```

```html
  <box box-title="Título do box">
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

> ### <a name="componentesNgProdeb"></a> Componentes NgProdeb ###

- Para saber como usar os componentes acesse: [Git NgProdeb](http://git.prodeb.ba.gov.br/ngprodeb)

> ### <a name="icons"></a> Ícones ###

- Os icones usados no sistema são encontrados em [Material Icons](https://design.google.com/icons/) e seguem o padrão abaixo:

```html
<md-icon md-font-set="material-icons">3d_rotation</md-icon>
```

## <a name="log"></a> Log ##

> Para ver os logs

- acesse [http://localhost:5000/developer/log-viewer](http://localhost:5000/developer/log-viewer)
- digite o usuário conforme a variável de ambiente no arquivo .env DEVELOP_ID
- digite a senha conforme a variável de ambiente no arquivo .env DEVELOP_PASSWORD

## <a name="producao"></a> Produção ##

- altere os dados do arquivo .env.production com as configurações de produção (banco, smtp, nível de log, ftp e etc) e desative o debug.
- rode o comando **npm run package** (prepara a aplicação para produção, minificando os arquivos js, css e modificando o index.html para apontar para os arquivos minificados)
- os sistema irá perguntar se deseja enviar para o ftp, caso queria o pacote será enviado
- caso contrário o arquivo **{NomeProjeto}.tar.gz** constará na raiz do projeto para o devido uso