(function() {

  'use strict';

  angular
    .module('app')
    .controller('AuthController', AuthController);

  AuthController.$inject = ['$state', 'Auth', 'Global', 'Toast'];

  function AuthController($state, Auth, Global, Toast) {
    var vm = this;

    vm.login = login;

    activate();

    function activate() {
      var credentials = {};
    }

    function login() {
      var credentials = {
        email: vm.email,
        password: vm.password
      };

      Auth.login(credentials).then(function(data) {
        $state.go(Global.homeState);
      }, function(error) {
        var message = (error.data.error === 'invalid_credentials') ? "Credenciais inválidas" : "Não foi possível realizar o login";
        Toast.error(message);
      });
    }
  }

})();
