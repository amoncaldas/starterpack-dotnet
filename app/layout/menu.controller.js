(function () {

  'use strict';

  angular
    .module('app')
    .controller('MenuController', MenuController);

  /** @ngInject */
  function MenuController($mdSidenav, $state) {
    var vm = this;

    //Bloco de declaracoes de funcoes
    vm.open = open;
    vm.openMenuOrRedirectToState = openMenuOrRedirectToState;

    activate();

    function activate() {
      var menuPrefix = 'views.layout.menu.';

      // Array contendo os itens que são mostrados no menu lateral
      vm.itensMenu = [
        { state: 'dashboard', titulo: menuPrefix + 'dashboard', icon: 'dashboard', subItens: [] },
        {
          state: '#', titulo: menuPrefix + 'examples', icon: 'view_carousel', profiles: ['admin'],
          subItens: [
            { state: 'project', titulo: menuPrefix + 'project', icon: 'star' }
          ]
        },
        // Coloque seus itens de menu a partir deste ponto
        {
          state: '#', titulo: menuPrefix + 'admin', icon: 'settings_applications', profiles: ['admin'],
          subItens: [
            { state: 'user', titulo: menuPrefix + 'user', icon: 'people' },
            { state: 'mail', titulo: menuPrefix + 'mail', icon: 'mail' },
            { state: 'audit', titulo: menuPrefix + 'audit', icon: 'storage' },
            { state: 'dinamic-query', titulo: menuPrefix + 'dinamicQuery', icon: 'location_searching' }
          ]
        }
      ];
    }

    function open() {
      $mdSidenav('left').toggle();
    }

    /**
     * Método que exibe o sub menu dos itens do menu lateral caso tenha sub itens
     * caso contrário redireciona para o state passado como parâmetro
     */
    function openMenuOrRedirectToState($mdOpenMenu, ev, item) {
      if (angular.isDefined(item.subItens) && item.subItens.length > 0) {
        $mdOpenMenu(ev);
      } else {
        $state.go(item.state);
        $mdSidenav('left').close();
      }
    }

  }

})();
