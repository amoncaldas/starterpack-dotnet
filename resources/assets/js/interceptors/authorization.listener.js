(function () {
  'use strict';

  angular
    .module('app')
    .run(authorizationListener);

  function authorizationListener($rootScope, $state, Global, Auth) {

    $rootScope.$on('$stateChangeStart', function(event, toState) {
      if( toState.data && toState.data.needAdmin && !Auth.isAdmin() ) {
        $state.go(Global.notAuthorizedState);
        event.preventDefault();
      }

    });
  }
}());
