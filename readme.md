# Sobre #

- Este projeto tem como objetivo servir de base para futuros projetos da Prodeb.
- O mesmo utiliza Laravel 5.1 com Angular 1.5.
- O sistema utiliza [JWT](http://jwt.io) para autenticação através da lib [tymon/jwt-auth](https://github.com/tymondesigns/jwt-auth)
- O sistema não faz uso de sessão para identificação do usuário, toda a informação é através do token enviado/recebido
- Todas as funcionalidades retornam json contendo as informações da requisitadas.

# Pré requisitos #

- Preferencialmente utilize o Linux com o gerenciador APT
- node versão 4 ou superior [tutorial para instalar](https://nodejs.org/en/download/package-manager/)
- php versão 5.6 ou superior [tutorial para instalar](http://tecadmin.net/install-php5-on-ubuntu/)
- extenções php_fileinfo, php_mbstring, php_pdo_pgsql, php_pgsql, php_openssl
- composer [tutorial para instalar](https://getcomposer.org/doc/00-intro.md#globally)
- editor decente [vscode](https://code.visualstudio.com/) ou [atom.io](https://atom.io/)
- postgres

# Componentes #

> Componentes e frameworks disponíveis no projeto

- [AngularJS 1.5](https://angularjs.org)
- [Angular Material 1.1.0 (ou superior)](https://material.angularjs.org)
- [NgProdeb 0.1.2](git@git.prodeb.ba.gov.br:ngprodeb.git)
 
# Configuração #

> Rode os comandos abaixo.
> Todos os comandos devem ser executados no terminal do linux. No caso do windows utilize o Git Bash.

- git clone git@git.prodeb.ba.gov.br:thiagoantonius.souza/laravel_angular_base.git
- cd laravel_angular_base
- composer global require "laravel/installer=~1.1"
- npm install
- chmod **777** -R storage
- chmod **777** -R bootstrap/cache
- cp .env.example .env

> Configure o .env com os dados da conexão do postgres

- composer install
- php artisan key:generate
- php artisan jwt:secret
- php artisan migrate --seed

> Execute o comando abaixo para processar os arquivos .sass e concatenar os .js e .css injetando no index.html.
> O comando fica observando futuras modificações e repetindo o processo automaticamente 

- gulp
  - parametros opcionais 
    - **--sync** (Mantém o navegador sincronizado com as mudanças. O mesmo vai dar refresh automaticamente a cada mudança nos .js e .html )

# Uso #

> Em outra aba do terminal.

- npm run server (Este comando inicia o servidor php na porta 5000)

> Abra o navegador

- acesse http://localhost:5000
- logue com os dados email: **admin-base@prodeb.com** senha: **Prodeb01**

> Outros comandos

- gulp check (verifica a formatação do código javascript) 
  - parametros opcionais 
    - **--fix** (para corrigir os erros que podem ser corrigidos automaticamente)

> Use o gerador de estrutura de arquivo para gerar os arquivos necessários para o recurso,
> foi usado o [YEOMAN](http://yeoman.io) para criar o gerador.

- cd laravel_angular_base
- yo ngprodeb
- escolha a estrutura na lista
- digite o nome do recurso

#### (Para mais detalhes sobre o uso do gerador acesse  [PrGenerator](git@git.prodeb.ba.gov.br:generator-generator-prgenerator.git))

> ### Adicionando novo módulo angular ###

- adicione a dependência no arquivo bower.json
- adicione o caminho da dependência no arquivo gulpfile.js
  - para importação angular adicione no array 'paths.angularScripts'
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

- acesse o arquivo /public/client/app/app.external.js
- adicione a linha ```.constant('NOME_CONSTANTE', NOME_BIBLIOTECA);```

> ### Constantes ###

- acesse o arquivo /public/client/app/app.global.js
- adicione um novo atributo contendo o nome da constante e o seu valor

> ### Menu (adicionando itens ao menu) ###

- acesse o arquivo /public/client/app/layout/menu.controller.js
- adicione um objeto no array ```vm.itensMenu```<br>
  > exemlo do objeto sem sub-item:<br>
  ```
  { url: 'dashboard', titulo: menuPrefix + 'dashboard', icon: 'dashboard', subItens: [] }
  ```
  > exemplo do objeto com sub-itens:<br>
  ```
  {
    url: '#', titulo: menuPrefix + 'admin', icon: 'settings_applications', profiles: ['admin'],
    subItens: [
      { url: 'user', titulo: menuPrefix + 'user', icon: 'people' },
      { url: 'mail', titulo: menuPrefix + 'mail', icon: 'mail' },
      { url: 'audit', titulo: menuPrefix + 'audit', icon: 'storage' }
    ]
  }
  ```

> ### Internacionalização ###

- todas as strings usadas no sistema devem ser armazenadas no objeto data localizado no arquivo /public/client/app/core/language-loader.service.js
- estrutura do arquivo:
  - no primeiro momento estão as strings comuns ao sistema como um todo
  - em seguida as strings comuns aos formulários
  - strings aos dialogs
  - strings das mensagens do toast
  - strings dos breadcrumbs
  - strings comuns a todos os models(recurso)
  - strings comuns aos controllers
  - por fim as strings comuns a cada recurso

> ### Convenções ###
> (convenções adotadas para padronização do projeto)

- o conjunto de arquivos são chamados de recurso(resource) localizados sempre no caminho /public/client/app
- cada recurso pode pussuir os seguintes arquivos:
  - recursos.html(index)
  - recursos-list.html
  - recursos-form.html
  - recursos.controller.js
  - recursos.route.js
  - recursos.service.js
- deve ser usado o gerador de estrutura de arquivos para gerar os arquivos no padrão informado acima
- no lado servidor ao ser criado o controller deve-se mudar a heranças de Controller para CrudController
o mesmo acontece quando um model é criado deve-ser mudar a herança de Model para BaseModel
- as imagens devem ser armazenadas no caminho /public/client/images
- para alterar as propriedades de css acesse o arquivo /public/client/styles/app.scss
- os templates dos emails devem ser salvos no caminho /resources/views/mais

### Exemplos de uso ###
___

> Existe exemplos de funcionalidade utilizando os componentes fornecidos, estes podem ser encontrados
> na pasta /public/client/app/samples.

> ### Exemplo de implementação de alguns dos componentes que podem ser usados no projeto. ###

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

- __Box__<br>
(obs.: o box deve estar dentro de um ContentBody)

```html
  <box box-title="Titúlo do box">
    <box-toolbar-buttons>
      Botões no toolbar do box (Opcional)
    </box-toolbar-buttons>

      Conteúdo do box

    <box-footer-buttons>
      Botões no rodapé do box (Opcional)
    </box-footer-buttons>
  </box>
```

- ( para mais exemplos consulte /public/client/app/samples ) 

> ### Componentes NgProdeb ###

- Para saber como usar os componentes acesse: [Git NgProdeb](https://StudioEFE@bitbucket.org/thiagoaos/ngprodeb.git)
> ### Erro no watch no linux (rode os comandos no terminal) ###

- > watch:
  - echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p
- > sh: 1: node: not found
  - npm install nodejs-legacy

# Log #

> Para ver os logs

- acesse http://localhost:5000/developer/log-viewer
- digite o usuário conforme a variável de ambiente no arquivo .env DEVELOP_ID
- digite a senha conforme a variável de ambiente no arquivo .env DEVELOP_PASSWORD


# Produção #

- Remover a pasta /public/client/app/samples. 
- npm run package (prepara a aplicação para produção, minificando os arquivos js, css e modificando o index.html para apontar para os arquivos minificados)