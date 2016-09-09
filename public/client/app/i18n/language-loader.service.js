(function() {

  'use strict';

  angular
    .module('app')
    .factory('languageLoader', LanguageLoader);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function LanguageLoader($q, SupportService) {
    // return loaderFn
    return function() {
      var deferred = $q.defer();
      // do something with $http, $q and key to load localization files

      SupportService.langs().then(function(langs) {
        var data = {
          loading: 'Carregando...',
          processing: 'Processando...',
          list: 'Listar',
          add: 'Adicionar',
          yes: 'Sim',
          no: 'Não',
          all: 'Todos',
          notFound: 'Nenhum registro encontrado',
          form: {
            titleRegister: 'Formulário de Cadastro',
            titleUpdate: 'Formulário de Atualização',
            send: 'Enviar',
            save: 'Salvar',
            clear: 'Limpar',
            filter: 'Filtrar',
            search: 'Pesquisar',
            edit: 'Editar',
            cancel: 'Cancelar',
            update: 'Atualizar',
            remove: 'Remover',
            getOut: 'Sair'
          },
          dialog: {
            confirmTitle: 'Confirmação',
            confirmDescription: 'Confirma a ação?'
          },
          messages: {
            logoutInactive: 'Você foi deslogado do sistema por inatividade. Favor entrar no sistema novamente.'
          },
          breadcrumbs: {
            user: 'Administração - Usuário',
            dashboard: 'Dashboard',
            'user-profile': 'Perfil',
            audit: 'Administração - Auditoria',
            mail: 'Administração - Envio de e-mail',
            project: 'Exemplos - Projetos'
          },
          models: {
            user: 'Usuário'
          },
          controllers: {
            crud: {
              searchError: 'Não foi possível realizar a busca.',
              saveSuccess: 'Operação realizada com sucesso',
              saveError: 'Não foi possível salvar.',
              removeSuccess: 'Remoção realizada com sucesso.',
              removeError: 'Não foi possível remover.'
            }
          },
          audit: {
            title: '@:form.search',
            dateStart: 'Data Inicial',
            dateEnd: 'Data Final',
            resource: 'Recurso',
            user: '@:models.user',
            viewDetail: 'Visualizar Detalhamento'
          },
          login: {
            email: 'Email',
            password: 'Senha',
            in: 'Entrar',
            resetPassword: 'Esqueci minha senha',
            resetForm: {
              title: 'Redefinir minha senha',
              email: '@:login.email',
              password: '@:login.password',
              confimPassword: 'Confirmar senha'
            },
            invalidCredentials: 'Credenciais Inválidas',
            errorLogin: 'Não foi possível realizar o login'
          },
          dashboard: {
            title: 'Página inicial'
          },
          layout: {
            logout: '@:form.getOut',
            perfil: 'Perfil',
            menu: {
              dashboard: 'Dashboard',
              project: 'Projetos',
              admin: 'Administração',
              examples: 'Exemplos',
              user: 'Usuários',
              mail: 'Enviar e-mail',
              audit: 'Auditoria'
            }
          },
          mail: {
            title: 'Enviar e-mail',
            form: {
              to: 'Para',
              subject: 'Assunto',
              message: 'Mensagem'
            },
            userExists: 'Usuário já adicionado!',
            mailErrors: 'Ocorreu um erro nos seguintes emails abaixo:\n',
            sendMailSuccess: 'Email enviado com sucesso!',
            sendMailError: 'Não foi possível enviar o email.'
          },
          project: {
            listTask: 'Listar Tarefas',
            confirm: {
              title: 'Confirmar remoção',
              description: 'Deseja remover permanentemente o projeto {{name}}?'
            }
          },
          task: {
            title: 'Lista de Tarefas',
            ariaLabel: 'Tarefas',
            priority: 'Prioridade',
            description: 'Descrição',
            toggleDoneSuccess: '@:controllers.crud.saveSuccess',
            toggleDoneError: '@:controllers.crud.saveError',
            confirm: {
              title: '@:project.confirm.title',
              description: 'Deseja remover permanentemente a tarefa selecionada?'
            }
          },
          user: {
            arialLabel: 'Usuários',
            dialog: {
              title: 'Lista de Usuários',
              tranfer: 'Transferir'
            },
            profile: {
              updateError: 'Não foi possível atualizar seu profile',
              form: {
                name: '@:user.form.name',
                email: '@:user.form.email',
                password: 'Senha',
                confirmPassword: 'Confirmação da Senha'
              }
            },
            form: {
              name: 'Nome',
              email: 'Email'
            },
            confirm: {
              title: '@:project.confirm.title',
              description: 'Deseja remover permanentemente o usuário {{name}}?'
            },
            beforeRemoveError: 'Você não pode remover seu próprio usuário'
          }
        }

        //Merge com os langs definidos no servidor
        data = angular.merge(data, langs);

        return deferred.resolve(data);
      });

      return deferred.promise;
    }
  }

})();
