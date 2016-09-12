(function () {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController() {
    var vm = this;

    //Bloco de declaracoes de funcoes
    vm.openMenu = openMenu;

    activate();

    function activate() {
      var menuPrefix = 'layout.menu.';

      vm.itensMenu = [
        { url: 'dashboard', titulo: menuPrefix + 'dashboard', icon: 'dashboard', subItens: [] },
        {
          url: '#', titulo: menuPrefix + 'examples', icon: 'view_carousel', profiles: ['admin'],
          subItens: [
            { url: 'project', titulo: menuPrefix + 'project', icon: 'star' }
          ]
        },
        {
          url: '#', titulo: menuPrefix + 'admin', icon: 'settings_applications', profiles: ['admin'],
          subItens: [
            { url: 'user', titulo: menuPrefix + 'user', icon: 'people' },
            { url: 'mail', titulo: menuPrefix + 'mail', icon: 'mail' },
            { url: 'audit', titulo: menuPrefix + 'audit', icon: 'storage' },
            { url: 'dinamic-query', titulo: menuPrefix + 'dinamicQuery', icon: 'storage' }
          ]
        }
      ];
    }

    function openMenu($mdOpenMenu, ev, subItens) {
      if (subItens > 0) {
        $mdOpenMenu(ev);
      }
    }

  }

})();
