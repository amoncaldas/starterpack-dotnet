(function() {

  'use strict';

  angular
    .module('app')
    .controller('AppController', AppController);

  /** @ngInject */
  function AppController($state, Auth, Global, $mdSidenav) {
    var vm = this;

    vm.anoAtual = null;
    vm.isMobile = false;

    //Bloco de declaracoes de funcoes
    vm.openNavBar = openNavBar;
    vm.logout     = logout;

    activate();

    function activate() {
      var date = new Date();

      vm.anoAtual = date.getFullYear();
      if (screen.width < 1025) {
        vm.isMobile = true;
      }
    }

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
