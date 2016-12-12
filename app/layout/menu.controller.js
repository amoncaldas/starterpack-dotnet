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
        { state: 'app.dashboard', title: menuPrefix + 'dashboard', icon: 'dashboard', subItens: [] },
        {
          state: '#', title: menuPrefix + 'examples', icon: 'view_carousel', profiles: ['admin'],
          subItens: [
            { state: 'app.project', title: menuPrefix + 'project', icon: 'star' }
          ]
        },
        // Coloque seus itens de menu a partir deste ponto
        {
          state: '#', title: menuPrefix + 'admin', icon: 'settings_applications', profiles: ['admin'],
          subItens: [
            { state: 'app.user', title: menuPrefix + 'user', icon: 'people' },
            { state: 'app.mail', title: menuPrefix + 'mail', icon: 'mail' },
            { state: 'app.audit', title: menuPrefix + 'audit', icon: 'storage' },
            { state: 'app.dinamic-query', title: menuPrefix + 'dinamicQuery', icon: 'location_searching' }
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
