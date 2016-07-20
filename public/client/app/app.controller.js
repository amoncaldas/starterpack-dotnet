(function() {

  'use strict';

  angular
    .module('app')
    .controller('AppController', AppController);

  /** @ngInject */
  function AppController($state, Auth, Global) {
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
