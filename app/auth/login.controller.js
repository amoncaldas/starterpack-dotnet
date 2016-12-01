(function() {

  'use strict';

  angular
    .module('app')
    .controller('LoginController', LoginController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function LoginController($state, Auth, Global, PrToast, PrDialog, $translate) {
    var vm = this;

    vm.login = login;
    vm.openDialogResetPass = openDialogResetPass;

    activate();

    function activate() {
      vm.credentials = {};
    }

    function login() {
      var credentials = {
        email: vm.credentials.email,
        password: vm.credentials.password
      };

      Auth.login(credentials).then(function() {
        $state.go(Global.homeState);
      }, function(error) {
        if (error.data.error === 'invalid_credentials') {
          PrToast.error($translate.instant('messages.login.invalidCredentials'));
        } else {
          PrToast.errorValidation(error.data, $translate.instant('messages.login.errorLogin'));
        }
      });
    }

    /**
     * Exibe o dialog para recuperação de senha
     */
    function openDialogResetPass() {
      var config = {
        templateUrl: Global.clientPath + '/auth/send-reset-dialog.html',
        controller: 'PasswordController as passCtrl',
        hasBackdrop: true
      }

      PrDialog.custom(config);
    }

  }

})();
