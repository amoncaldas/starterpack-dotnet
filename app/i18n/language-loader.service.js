(function() {

  'use strict';

  angular
    .module('app')
    .factory('languageLoader', LanguageLoader);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function LanguageLoader($q, SupportService, $log) {

    function translations(langs) {
      var data = {
        loading: 'Carregando...',
        processing: 'Processando...',
        yes: 'Sim',
        no: 'Não',
        all: 'Todos',
        views: {
          breadcrumbs: {
            user: 'Administração - Usuário',
            'user-profile': 'Perfil',
            dashboard: 'Dashboard',
            audit: 'Administração - Auditoria',
            mail: 'Administração - Envio de e-mail',
            project: 'Exemplos - Projetos',
            'dinamic-query': 'Administração - Consultas Dinâmicas',
            'not-authorized': 'Acesso Negado'
          },
          titles: {
            dashboard: 'Página inicial',
            mailSend: 'Enviar e-mail',
            taskList: 'Lista de Tarefas',
            userList: 'Lista de Usuários',
            auditList: 'Lista de Logs',
            register: 'Formulário de Cadastro',
            resetPassword: 'Redefinir Senha',
            update: 'Formulário de Atualização'
          },
          actions: {
            send: 'Enviar',
            save: 'Salvar',
            clear: 'Limpar',
            clearAll: 'Limpar Tudo',
            restart: 'Reiniciar',
            filter: 'Filtrar',
            search: 'Pesquisar',
            list: 'Listar',
            edit: 'Editar',
            cancel: 'Cancelar',
            update: 'Atualizar',
            remove: 'Remover',
            getOut: 'Sair',
            add: 'Adicionar',
            in: 'Entrar',
            loadImage: 'Carregar Imagem'
          },
          fields: {
            date: 'Data',
            action: 'Ação',
            actions: 'Ações',
            audit: {
              dateStart: 'Data Inicial',
              dateEnd: 'Data Final',
              resource: 'Recurso',
              allResources: 'Todos Recursos',
              type: {
                created: 'Cadastrado',
                updated: 'Atualizado',
                deleted: 'Removido'
              }
            },
            login: {
              resetPassword: 'Esqueci minha senha',
              confirmPassword: 'Confirmar senha'
            },
            mail: {
              to: 'Para',
              subject: 'Assunto',
              message: 'Mensagem'
            },
            queryDinamic: {
              filters: 'Filtros',
              results: 'Resultados',
              model: 'Model',
              attribute: 'Atributo',
              operator: 'Operador',
              resource: 'Recurso',
              value: 'Valor',
              operators: {
                equals: 'Igual',
                diferent: 'Diferente',
                conteins: 'Contém',
                startWith: 'Inicia com',
                finishWith: 'Finaliza com',
                biggerThan: 'Maior',
                equalsOrBiggerThan: 'Maior ou Igual',
                lessThan: 'Menor',
                equalsOrLessThan: 'Menor ou Igual'
              }
            },
            project: {
              totalTask: 'Total de Tarefas'
            },
            task: {
              done: 'Não Feito / Feito'
            },
            user: {
              perfils: 'Perfis',
              nameOrEmail: 'Nome ou Email'
            }
          },
          layout: {
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
          tooltips: {
            audit: {
              viewDetail: 'Visualizar Detalhamento'
            },
            user: {
              perfil: 'Perfil',
              transfer: 'Transferir'
            },
            task: {
              listTask: 'Listar Tarefas'
            }
          }
        },
        attributes: {
          email: 'Email',
          password: 'Senha',
          name: 'Nome',
          image: 'Imagem',
          roles: 'Perfis',
          //é carregado do servidor caso esteja definido no mesmo
          auditModel: {
          }
        },
        dialog: {
          confirmTitle: 'Confirmação',
          confirmDescription: 'Confirma a ação?',
          removeDescription: 'Deseja remover permanentemente {{name}}?',
          audit: {
            created: 'Informações do Cadastro',
            updatedBefore: 'Antes da Atualização',
            updatedAfter: 'Depois da Atualização',
            deleted: 'Informações antes de remover'
          },
          login: {
            resetPassword: {
              description: 'Digite abaixo o email cadastrado no sistema.'
            }
          }
        },
        messages: {
          internalError: 'Ocorreu um erro interno, contate o administrador do sistema',
          notFound: 'Nenhum registro encontrado',
          searchError: 'Não foi possível realizar a busca.',
          saveSuccess: 'Registro salvo com sucesso',
          operationSuccess: 'Operação realizada com sucesso',
          operationError: 'Erro ao realizar a operação',
          saveError: 'Erro ao tentar salvar o registro.',
          removeSuccess: 'Remoção realizada com sucesso.',
          removeError: 'Erro ao tentar remover o registro.',
          validate: {
            fieldRequired: 'O campo {{field}} é obrigratório.'
          },
          layout: {
            error404: 'Você não tem acesso a esta página.'
          },
          login: {
            logoutInactive: 'Você foi deslogado do sistema por inatividade. Favor entrar no sistema novamente.',
            invalidCredentials: 'Credenciais Inválidas',
            errorLogin: 'Não foi possível realizar o login'
          },
          dashboard: {
            welcome: 'Seja bem Vindo {{userName}}',
            description: 'Utilize o menu para navegação.'
          },
          mail: {
            mailErrors: 'Ocorreu um erro nos seguintes emails abaixo:\n',
            sendMailSuccess: 'Email enviado com sucesso!',
            sendMailError: 'Não foi possível enviar o email.'
          },
          user: {
            beforeRemoveError: 'Você não pode remover seu próprio usuário',
            userExists: 'Usuário já adicionado!',
            profile: {
              updateError: 'Não foi possível atualizar seu profile'
            }
          },
          queryDinamic: {
            noFilter: 'Nenhum filtro adicionado'
          }
        },
        models: {
          user: 'Usuário',
          task: 'Tarefa',
          project: 'Projeto'
        }
      }

      //Merge com os langs definidos no servidor
      data = angular.merge(data, langs);

      return data;
    };

    // return loaderFn
    return function(options) {
      $log.info('Carregando o conteudo da linguagem ' + options.key);

      var deferred = $q.defer();

      //Carrega as langs que precisam e estão no servidor para não precisar repetir aqui
      SupportService.langs().then(function(langs) {
        var data = translations(langs);

        return deferred.resolve(data);
      }, function() {
        var data = translations([]);

        return deferred.resolve(data);
      });

      return deferred.promise;
    }
  }

})();
