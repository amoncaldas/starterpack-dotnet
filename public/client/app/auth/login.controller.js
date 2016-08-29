(function() {

  'use strict';

  angular
    .module('app')
    .controller('LoginController', LoginController);

  /** @ngInject */
  function LoginController($state, Auth, Global, PrToast, PrDialog) {
    var vm = this;

    vm.login = login;

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
          ? 'Credenciais inválidas'
          : 'Não foi possível realizar o login';

        PrToast.error(message);

        PrDialog.close();

      });
    }
  }

})();
