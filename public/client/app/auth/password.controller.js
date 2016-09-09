(function () {

  'use strict';

  angular
    .module('app')
    .controller('PasswordController', PasswordController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function PasswordController(Global, $stateParams, $http, $timeout, $state, PrToast, PrDialog, Auth) {
    var vm = this;

    vm.sendReset = sendReset;
    vm.closeDialog = closeDialog;
    vm.cleanForm = cleanForm;
    vm.sendEmailResetPassword = sendEmailResetPassword;

    activate();

    function activate() {
      vm.reset = { email: '', token: $stateParams.token };
    }

    function sendReset() {
      $http.post(Global.apiVersion + '/password/reset', vm.reset)
        .then(function (response) {
          PrToast.success(response.data.msg);
          $timeout(function () {
            $state.go(Global.loginState);
          }, 3000);
        }, function (error) {
          if (error.status === 400) {
            PrToast.error(error.data.msg);
            $timeout(function () {
              $state.go(Global.loginState);
            }, 2000);
          } else if (error.status === 500) {
            PrToast.error('Ocorreu um erro interno, contate o administrador do sistema');
          } else {
            var msg = '';

            for (var i = 0; i < error.data.password.length; i++) {
              msg += error.data.password[i] + '<br>';
            }
            PrToast.error(msg.toUpperCase());
          }
        });
    }

    function closeDialog() {
      PrDialog.close();
    }

    function cleanForm() {
      vm.reset.email = '';
    }

    function sendEmailResetPassword() {

      if (vm.reset.email === '') {
        PrToast.error('O campo email é obrigratório.');
        return;
      }

      Auth.sendEmailResetPassword(vm.reset).then(function (response) {
        PrToast.success(response.data.msg);
        vm.cleanForm();
        vm.closeDialog();
      }, function (error) {
        PrToast.error(error.data.msg);
      });
    }

  }

})();
