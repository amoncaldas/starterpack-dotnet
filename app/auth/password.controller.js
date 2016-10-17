(function () {

  'use strict';

  angular
    .module('app')
    .controller('PasswordController', PasswordController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function PasswordController(Global, $stateParams, $http, $timeout, $state, PrToast, PrDialog, Auth, $translate) {
    var vm = this;

    vm.sendReset = sendReset;
    vm.closeDialog = closeDialog;
    vm.cleanForm = cleanForm;
    vm.sendEmailResetPassword = sendEmailResetPassword;

    activate();

    function activate() {
      vm.reset = { email: '', token: $stateParams.token };
    }

    /**
     * Realiza a alteração da senha do usuário e o redireciona para a tela de login
     */
    function sendReset() {
      $http.post(Global.apiPath + '/password/reset', vm.reset)
        .then(function () {
          PrToast.success($translate.instant('messages.operationSuccess'));
          $timeout(function () {
            $state.go(Global.loginState);
          }, 1500);
        }, function (error) {
          if (error.status === 400) {
            PrToast.error(error.data.msg);
            $timeout(function () {
              $state.go(Global.loginState);
            }, 2000);
          } else if (error.status === 500) {
            PrToast.error($translate.instant('messages.internalError'));
          } else {
            var msg = '';

            for (var i = 0; i < error.data.password.length; i++) {
              msg += error.data.password[i] + '<br>';
            }
            PrToast.error(msg.toUpperCase());
          }
        });
    }

    /**
     * Envia um email de recuperação de senha com o token do usuário
     */
    function sendEmailResetPassword() {

      if (vm.reset.email === '') {
        PrToast.error($translate.instant('messages.validate.fieldRequired', { field: 'email' }));
        return;
      }

      Auth.sendEmailResetPassword(vm.reset).then(function () {
        PrToast.success($translate.instant('messages.operationSuccess'));
        vm.cleanForm();
        vm.closeDialog();
      }, function (error) {
        PrToast.error(error.data.msg);
      });
    }

    function closeDialog() {
      PrDialog.close();
    }

    function cleanForm() {
      vm.reset.email = '';
    }

  }

})();
