(function () {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController($translate) {
    var vm = this;

    //Bloco de declaracoes de funcoes
    vm.openMenu = openMenu;

    activate();

    function activate() {
      vm.itensMenu = [
        { url: 'dashboard', titulo: $translate.instant('layout.menu.dashboard'), icon: 'dashboard', subItens: [] },
        { url: 'project', titulo: 'Projetos', icon: 'star', subItens: [] },
        {
          url: '#', titulo: 'Administração', icon: 'settings_applications', profiles: ['admin'],
          subItens: [
            { url: 'user', titulo: 'Usuários', icon: 'people' },
            { url: 'mail', titulo: 'Envio de e-mail', icon: 'mail' },
            { url: 'audit', titulo: 'Auditoria', icon: 'storage' }
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
