(function() {

  'use strict';

  angular
    .module('app')
    .controller('MailsController', MailsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MailsController(MailsService, UsersService, PrDialog, PrToast, $q, lodash, $translate, Global) {
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

      UsersService.query({
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
        templateUrl: Global.clientPath + '/users/dialog/users-dialog.html',
        hasBackdrop: true
      };

      PrDialog.custom(config);

    }

    function addUserMail(user) {
      var users = lodash.find(vm.mail.users, { email: user.email });

      if (vm.mail.users.length > 0 && angular.isDefined(users)) {
        PrToast.warn($translate.instant('mail.userExists'));
      } else {
        vm.mail.users.push({ name: user.name, email: user.email })
      }
    }

    function send() {

      vm.mail.$save().then(function(response) {
        if (response.length > 0) {
          var msg = $translate.instant('mail.mailErrors');

          for (var i=0; i < response.length; i++) {
            msg += response + '\n';
          }
          PrToast.error(msg);
          vm.cleanForm();
        } else {
          PrToast.success($translate.instant('mail.sendMailSuccess'));
          vm.cleanForm();
        }
      }, function(response) {
        PrToast.errorValidation(response.data, $translate.instant('mail.sendMailError'));
      });
    }

    function cleanForm() {
      vm.mail = new MailsService();
      vm.mail.users = [];
    }

  }

})();
