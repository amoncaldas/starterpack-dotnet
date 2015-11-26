(function() {

  'use strict';

  angular
    .module('app')
    .controller('GlobalController', GlobalController);

  GlobalController.$inject = ['$state', '$http', 'Auth', 'Global'];

  function GlobalController($state, $http, Auth, Global) {
    var vm = this;

    vm.logout = logout;

    activate();

    function activate() {}

    function logout() {
      Auth.logout().then(function() {
        $state.go(Global.loginState);
      });
    }
  }

})();
