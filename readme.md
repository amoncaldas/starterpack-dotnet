
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
- O mesmo utiliza Angular 1.5.
- O sistema utiliza [JWT](http://jwt.io) para autenticação
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam json contendo as informações da requisitadas.

- Este repositório só tem o front-end. O mesmo deve ser conectado com um dos back-ends existentes.

## Pré requisitos ##

- Preferencialmente utilize o Linux com o gerenciador APT
- editor decente [vscode](https://code.visualstudio.com/) ou [atom.io](https://atom.io/)
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
    - Configure o npm para rodar sem sudo [tutorial](https://docs.npmjs.com/getting-started/fixing-npm-permissions)

## Componentes ##

> Componentes e frameworks utilizados no projeto

- [AngularJS](https://angularjs.org)
- [Angular Material](https://material.angularjs.org)
- [NgProdeb](git@git.prodeb.ba.gov.br:ngprodeb.git)
- [momentjs](http://momentjs.com/)

## Instalação ##

> Você vai precisar de permissão para os repositórios Starter Pack PHP Angular, ngProdeb, php-docker e Generator NgProdeb
> Rode os comandos abaixo no terminal do linux.

```sh
git clone git@git.prodeb.ba.gov.br:starter-pack-angular-client.git client
cd {client}
```

> Instale todos os pre requisitos (php, node, composer ...) antes de seguir
> Em uma instalação limpa do Linux Mint ou Ubuntu os comandos a seguir instalam os pré requisitos

```sh

curl -sL https://deb.nodesource.com/setup_4.x | sudo bash -

sudo apt-get update && sudo apt-get install -y build-essential libxml2-dev libfreetype6-dev libjpeg-turbo8-dev libmcrypt-dev libpng12-dev libssl-dev libpq-dev git vim unzip nodejs

mkdir ~/.npm-global
npm config set prefix '~/.npm-global'

```

> adicione a linha a seguir no final do arquivo **~/.bashrc**

```
export PATH=~/.npm-global/bin:$PATH
```


```sh

source ~/.bashrc

npm install -g npm

echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p

npm install -g yo gulp gulp-babel babel-preset-es2015 eslint eslint-plugin-angular bower http-server

npm install
```

## Colocar para Rodar ##

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente

```sh
cd pasta_do_projeto
gulp
```

  - parametros opcionais
    - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )
  - No caso da instalação MANUAL pode ocorrer um erro sobre quantidade de arquivos observados no linux, para corrigir este erro execute os comandos abaixo no terminal
    - echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p
  - No caso da instalação com o DOCKER esse comando é executado automaticamente

> Em outra aba do terminal rode o comando abaixo para levantar o servidor php:

```sh
npm run server (Este comando inicia o servidor php na porta 8080)
```

> Abra o navegador

- acesse **http://localhost:8080**
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
      - eslint (para verificar erros de formatação e code smell)
      - editor config (para configurar codificação, tabulação ...)
      - beautify (para formatar o código)
      - vscode-icons
      - angular material snippets
      - auto close tag

> ### Gerador de Código ###

- Use o gerador de estrutura de arquivo para gerar os arquivos necessários para o recurso,

```sh
cd {pasta_do_projeto}
yo ngprodeb
```

- escolha a estrutura na lista
- digite o nome do recurso

**para mais detalhes sobre o uso do gerador acesse [Generator NgProdeb](http://git.prodeb.ba.gov.br/generator-ngprodeb/tree/master)**

> ### Adicionar novo módulo angular ###

- adicione a dependência no arquivo bower.json
- rode o comando

```sh
bower install {nome-da-biblioteca}
```

- adicione o caminho da dependência no arquivo gulpfile.js
  - para importação angular adicione no array **paths.angularScripts**
  - ao adicionar um novo módulo o gulp deve ser reiniciado
- adicione o módulo no arquivo {pasta_do_projeto}/app/app.js

> ### Configuração ###

- acesse o arquivo {pasta_do_projeto}/app/app.config.js
- $translateProvider
  - configura o módulo de tradução das strings
- moment.locale('');
  - configura o idioma das datas
- $mdThemingProvider
  - configura o tema do angular material

> ### Bibliotecas Externas ###
> (bibliotecas que não são módulos do angular)

- acesse o arquivo **{pasta_do_projeto}/app/app.external.js**
- adicione a linha:

```javascript
.constant('{NOME_DA_CONSTANTE}', {NOME_BIBLIOTECA});
```

> ### Constantes ###

- acesse o arquivo **{pasta_do_projeto}/app/app.global.js**
- adicione um novo atributo contendo o nome da constante e o seu valor

> ### Menu ###
(adicionando itens ao menu)

- acesse o arquivo **{pasta_do_projeto}/app/layout/menu.controller.js**
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

  - todas as strings usadas no sistema devem ser armazenadas no objeto data localizado no arquivo **{pasta_do_projeto}/app/i18n/language-loader.service.js**
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

  - o conjunto de arquivos são chamados de recurso(resource) localizados sempre no caminho **{pasta_do_projeto}/app**
  - cada recurso pode pussuir os seguintes arquivos:
    - recursos.html(index)
    - recursos-list.html
    - recursos-form.html
    - recursos.controller.js
    - recursos.route.js
    - recursos.service.js
  - deve ser usado o gerador de estrutura de arquivos para gerar os arquivos no padrão informado acima
  - as imagens devem ser armazenadas no caminho **{pasta_do_projeto}/images**
  - para alterar as propriedades de css acesse o arquivo **{pasta_do_projeto}/styles/app.scss**

> ### CRUD ###

**crud.controller.js** ({pasta_do_projeto}/app/core/crud.controller.js)

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

> ### Diretivas ###

O uso de todos os componentes são demonstrados através das funcionalidades de exemplo adiconadas na pasta **{pasta_do_projeto}/app/samples**

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

- ( para mais exemplos consulte **{pasta_do_projeto}/app/samples** )

> ### Componentes NgProdeb ###

- Para saber como usar os componentes acesse: [Git NgProdeb](http://git.prodeb.ba.gov.br/ngprodeb)

> ### Ícones ###

- Os icones usados no sistema são encontrados em [Material Icons](https://design.google.com/icons/) e seguem o padrão abaixo:

```html
<md-icon md-font-set="material-icons">
  3d_rotation
</md-icon>
```

## Produção ##

- rode o comando **gulp --production** 
    - Este commando minifica os arquivos js, css e modificando o index.html para apontar para os arquivos minificados