(function () {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController($mdSidenav) {
    var vm = this;

    //Bloco de declaracoes de funcoes
    vm.open = open;
    vm.openSubMenu = openSubMenu;

    activate();

    function activate() {
      var menuPrefix = 'layout.menu.';

      // Array contendo os itens que são mostrados no menu lateral
      vm.itensMenu = [
        { url: 'dashboard', titulo: menuPrefix + 'dashboard', icon: 'dashboard', subItens: [] },
        {
          url: '#', titulo: menuPrefix + 'examples', icon: 'view_carousel', profiles: ['admin'],
          subItens: [
            { url: 'project', titulo: menuPrefix + 'project', icon: 'star' }
          ]
        },
        // Coloque seus itens de menu a partir deste ponto
        {
          url: '#', titulo: menuPrefix + 'admin', icon: 'settings_applications', profiles: ['admin'],
          subItens: [
            { url: 'user', titulo: menuPrefix + 'user', icon: 'people' },
            { url: 'mail', titulo: menuPrefix + 'mail', icon: 'mail' },
            { url: 'audit', titulo: menuPrefix + 'audit', icon: 'storage' },
            { url: 'dinamic-query', titulo: menuPrefix + 'dinamicQuery', icon: 'location_searching' }
          ]
        }
      ];
    }

    function open() {
      $mdSidenav('left').toggle();
    }

    /**
     * Método que exibe o sub menu dos itens do menu lateral
     */
    function openSubMenu($mdOpenMenu, ev, subItens) {
      if (subItens > 0) {
        $mdOpenMenu(ev);
      }
    }

  }

})();
