(function () {
  'use strict';

  angular
    .module('app')
    .run(authorizationListener);

  /** @ngInject */
  function authorizationListener($rootScope, $state, Global, Auth) {
    /**
     * A cada mudança de estado ("página") verifica se o destino só pode ser acesso por um administrador
     * caso necessite, verifica se o usuário é administrador permitindo assim a mudança de página
     */
    $rootScope.$on('$stateChangeStart', function(event, toState) {
      if (toState.data && toState.data.needAdmin && Auth.currentUser && !Auth.currentUser.isAdmin()) {
        $state.go(Global.notAuthorizedState);
        event.preventDefault();
      }

    });
  }
}());
