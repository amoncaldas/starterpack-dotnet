(function() {

  'use strict';

  angular
    .module('app')
    .controller('AuthController', AuthController);

  AuthController.$inject = ['$state', '$http', '$rootScope', 'Auth', '$auth', 'GlobalService'];

  function AuthController($state, $http, $rootScope, Auth, $auth, GlobalService) {
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
        $state.go(GlobalService.homeState);
      }, function(error) {
        toastr.error(error.data.error);
      });
    }
  }

})();
