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

      //Carrega as langs que precisam e estão no servidor para não precisar repetir aqui
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
            clearAll: 'Limpar Tudo',
            restart: 'Reiniciar',
            filter: 'Filtrar',
            search: 'Pesquisar',
            edit: 'Editar',
            cancel: 'Cancelar',
            update: 'Atualizar',
            remove: 'Remover',
            getOut: 'Sair',
            add: 'Adicionar'
          },
          table: {
            th: {
              actions: 'Ações'
            }
          },
          attributes: {
            //é carregado do servidor
          },
          dialog: {
            confirmTitle: 'Confirmação',
            confirmDescription: 'Confirma a ação?'
          },
          messages: {
            logoutInactive: 'Você foi deslogado do sistema por inatividade. Favor entrar no sistema novamente.',
            internalError: 'Ocorreu um erro interno, contate o administrador do sistema',
            validate: {
              fieldRequired: 'O campo {{field}} é obrigratório.'
            }
          },
          breadcrumbs: {
            user: 'Administração - Usuário',
            dashboard: 'Dashboard',
            'user-profile': 'Perfil',
            audit: 'Administração - Auditoria',
            mail: 'Administração - Envio de e-mail',
            project: 'Exemplos - Projetos',
            'dinamic-query': 'Consultas Dinamicas',
            'not-authorized': 'Acesso Negado'
          },
          models: {
            user: 'Usuário',
            task: 'Tarefa',
            project: 'Projeto'
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
            viewDetail: 'Visualizar Detalhamento',
            type: {
              created: 'Cadastrado',
              updated: 'Atualizado',
              deleted: 'Removido'
            },
            table: {
              th: {
                user: '@:models.user',
                resource: 'Recurso',
                action: 'Ação',
                date: 'Data',
                actions: '@:table.th.actions'
              }
            },
            dialog: {
              title: {
                created: 'Informações do Cadastro',
                updatedBefore: 'Antes da Atualização',
                updatedAfter: 'Depois da Atualização',
                deleted: 'Informações antes de remover'
              }
            }
          },
          dinamicQuery: {
            filters: 'Filtros',
            results: 'Resultados',
            form: {
              attribute: 'Atributo',
              operator: 'Operador',
              resource: 'Recurso',
              value: 'Valor'
            },
            table: {
              th: {
                model: 'Model',
                attribute: '@:dinamicQuery.form.attribute',
                operator: '@:dinamicQuery.form.operator',
                resource: '@:dinamicQuery.form.resource',
                value: '@:dinamicQuery.form.value'
              }
            },
            operators: {
              label: {
                equals: 'Igual',
                diferent: 'Diferente',
                conteins: 'Contém',
                startWith: 'Inicia com',
                finishWith: 'Finaliza com',
                larger: 'Maior',
                largerOrEquals: 'Maior ou Igual',
                smaller: 'Menor',
                smallerOrEquals: 'Menor ou Igual'
              }
            }
          },
          login: {
            email: '@:attributes.email',
            password: '@:attributes.password',
            in: 'Entrar',
            resetPassword: 'Esqueci minha senha',
            invalidCredentials: 'Credenciais Inválidas',
            errorLogin: 'Não foi possível realizar o login',
            resetForm: {
              title: 'Redefinir minha senha',
              email: '@:login.email',
              password: '@:login.password',
              confimPassword: 'Confirmar senha'
            },
            resetDialog: {
              title: 'Redefinir minha senha',
              description: 'Digite abaixo o email cadastrado no sistema.'
            }
          },
          dashboard: {
            title: 'Página inicial',
            messages: {
              welcome: 'Seja bem Vindo {{userName}}',
              description: 'Utilize o menu para navegação.'
            }
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
              audit: 'Auditoria',
              dinamicQuery: 'Consultas Dinamicas'
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
            },
            table: {
              th: {
                name: '@:attributes.name',
                cost: '@:attributes.cost',
                totalTask: 'Total de Tarefas',
                created_at: '@:attributes.created_at',
                actions: '@:table.th.actions'
              }
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
            },
            table: {
              th: {
                description: '@:task.description',
                scheduled_to: '@:attributes.scheduled_to',
                priority: '@:task.priority',
                done: 'Não Feito / Feito',
                actions: '@:table.th.actions'
              }
            }
          },
          user: {
            arialLabel: 'Usuários',
            beforeRemoveError: 'Você não pode remover seu próprio usuário',
            dialog: {
              title: 'Lista de Usuários',
              transfer: 'Transferir'
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
            table: {
              th: {
                name: '@:attributes.name',
                email: '@:attributes.email',
                perfils: 'Perfis',
                actions: '@:table.th.actions'
              }
            }
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
