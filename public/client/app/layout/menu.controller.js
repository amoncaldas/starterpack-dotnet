(function() {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController() {
    var vm = this;

    vm.itensMenu = [
      { url: 'dashboard', titulo: 'Home', icon: 'home', subItens: [] },
      { url: '#', titulo: 'Administração', icon: 'settings_applications',
        subItens: [
          { url: 'user', titulo: 'Usuário', icon: 'people' },
          { url: 'auditoria', titulo: 'Auditoria', icon: 'font_download' }
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
