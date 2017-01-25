(function() {
  'use strict';

  angular
    .module('app')
    .run(authorizationListener);

  /** @ngInject */
  function authorizationListener($rootScope, $state, Global, Auth) {
    /**
     * A cada mudança de estado ("página") verifica se o usuário tem o perfil 
     * necessário para o acesso a mesma
     */
    $rootScope.$on('$stateChangeStart', function(event, toState) {
      if (toState.data && toState.data.needProfile && Auth.currentUser &&
        !Auth.currentUser.hasProfile(toState.data.needProfile, toState.data.allProfiles)) {

        $state.go(Global.notAuthorizedState);
        event.preventDefault();
      }

    });
  }
}());
