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
        var message = (error.data.error === 'invalid_credentials')
          ? $translate.instant('login.invalidCredentials')
          : $translate.instant('login.errorLogin');

        PrToast.error(message);
      });
    }

    function openDialogResetPass() {
      var config = {
        templateUrl: Global.clientPath + '/auth/reset-pass-dialog.html',
        controller: function(PrDialog) {
          var vm = this;
          vm.reset = {email: ''};

          vm.closeDialog = closeDialog;
          vm.cleanForm = cleanForm;
          vm.sendResetPass = sendResetPass;

          function closeDialog() {
            PrDialog.close();
          }

          function cleanForm() {
            vm.reset.email = '';
          }

          function sendResetPass() {
            Auth.sendResetPassword(vm.reset);
          }

        },
        controllerAs: 'ctrl',
        hasBackdrop: true
      }

      PrDialog.custom(config);

    }

  }

})();
