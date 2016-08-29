(function() {

  'use strict';

  angular
    .module('app')
    .controller('MailsController', MailsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MailsController(MailService, UserService, PrDialog, PrToast, $q, lodash) {
    var vm = this;

    vm.filterSelected = false;

    vm.loadUsers = loadUsers;
    vm.openUserDialog = openUserDialog;
    vm.addUserMail = addUserMail;
    vm.cleanForm = cleanForm;
    vm.send = send;

    activate();

    function activate() {
      vm.cleanForm();
    }

    /**
     * Realiza a busca pelo usuário remotamente
     * @params {string} - Recebe o valor para ser pesquisado
     * @return {promisse} - Retorna uma promisse que o componete resolve
     */
    function loadUsers(criteria) {
      var deferred = $q.defer();

      UserService.query({
        nameOrEmail: criteria,
        notUsers: lodash.map(vm.mail.users, lodash.property('id')).toString(),
        limit: 5
      }).then(function(data) {

        //data tem email
        data = lodash.filter(data, function(user) {
          return !lodash.find(vm.mail.users, { email: user.email });
        });

        deferred.resolve(data);
      });

      return deferred.promise;
    }

    function openUserDialog() {
      var config = {
        locals: {
          userDialogInput: {
            transferUserFn: vm.addUserMail
          }
        },
        controller: 'UsersDialogController',
        controllerAs: 'ctrl',
        templateUrl: '/users/dialog/users-dialog.html',
        hasBackdrop: true
      };

      PrDialog.custom(config);

    }

    function addUserMail(user) {
      var users = lodash.find(vm.mail.users, { email: user.email });

      if (vm.mail.users.length > 0 && angular.isDefined(users)) {
        PrToast.warn('Usuário já adicionado!');
      } else {
        vm.mail.users.push({ name: user.name, email: user.email })
      }
    }

    function send() {

      if (vm.mail.users.length === 0) {
        PrToast.warn('Informe ao menos um e-mail de destinatário.');
        return;
      }

      if (angular.isUndefined(vm.mail.subject)) {
        PrToast.warn('Campo assunto é obrigratório.');
        return;
      }

      if (angular.isUndefined(vm.mail.message)) {
        PrToast.warn('Campo mensagem é obrigratório.');
        return;
      }

      vm.mail.$save().then(function(response) {
        if (response.length > 0) {
          var msg = 'Ocorreu um erro nos seguintes emails abaixo:\n';

          for (var i=0; i < response.length; i++) {
            msg += response + '\n';
          }
          PrToast.error(msg);
          vm.cleanForm();
        } else {
          PrToast.success('Email enviado com sucesso!');
          vm.cleanForm();
        }
      });
    }

    function cleanForm() {
      vm.mail = new MailService();
      vm.mail.users = [];
    }

  }

})();
