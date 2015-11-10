(function() {

  'use strict';

  angular
    .module('app')
    .controller('GlobalController', GlobalController);

  GlobalController.$inject = ['$state', '$http', '$rootScope', 'Auth', '$auth', 'GlobalService'];

  function GlobalController($state, $http, $rootScope, Auth, $auth, GlobalService) {
    var vm = this;

    vm.logout = logout;

    activate();

    function activate() {}

    function logout() {
      Auth.logout().then(function() {
        $state.go(GlobalService.loginState);
      });
    }
  }

})();
