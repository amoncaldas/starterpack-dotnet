(function() {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController() {
    var vm = this;

    vm.itensMenu = [
      { url: 'dashboard', titulo: 'Dashboard', icon: 'dashboard', subItens: [] },
      { url: '#', titulo: 'Administração', icon: 'settings_applications',
        subItens: [
          { url: 'user', titulo: 'Usuários', icon: 'people' }
        ]
      }
    ];

    //Bloco de declaracoes de funcoes
    vm.openMenu   = openMenu;

    function openMenu($mdOpenMenu, ev, subItens) {
      if (subItens > 0) {
        $mdOpenMenu(ev);
      }
    }

  }

})();
