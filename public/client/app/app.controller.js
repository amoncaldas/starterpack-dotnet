(function() {

  'use strict';

  angular
    .module('app')
    .controller('AppController', AppController);

  /** @ngInject */
  function AppController($state, Auth, Global, $mdSidenav) {
    var vm = this;

    //Bloco de declaracoes de funcoes
    vm.openNavBar = openNavBar;
    vm.logout     = logout;

    activate();

    function activate() {}

    function openNavBar() {
      $mdSidenav('left').toggle();
    }

    function logout() {
      Auth.logout().then(function() {
        $state.go(Global.loginState);
      });
    }

  }

})();
