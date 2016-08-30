(function() {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MenuController($translate) {
    var vm = this;

    vm.itensMenu = [
      { url: 'dashboard', titulo: $translate.instant('layout.menu.dashboard'), icon: 'dashboard', subItens: [] },
      { url: 'project', titulo: $translate.instant('layout.menu.projects'), icon: 'star', subItens: [] },
      {
        url: '#', titulo: $translate.instant('layout.menu.admin'), icon: 'settings_applications', profiles: ['admin'],
        subItens: [
          { url: 'user', titulo: $translate.instant('layout.menu.users'), icon: 'people' },
          { url: 'mail', titulo: $translate.instant('layout.menu.sendMail'), icon: 'mail' },
          { url: 'audit', titulo: $translate.instant('layout.menu.audit'), icon: 'storage' }
        ]
      }
    ];

    //Bloco de declaracoes de funcoes
    vm.openMenu = openMenu;

    function openMenu($mdOpenMenu, ev, subItens) {
      if (subItens > 0) {
        $mdOpenMenu(ev);
      }
    }

  }

})();
