(function() {
  'use strict';

  angular
    .module('app')
    .run(authenticationListener);

  /**
   * Listen all state (page) changes. Every time a state change need to verify the user is authenticated or not to
   * redirect to correct page. When a user close the browser without logout, when him reopen the browser this event
   * reauthenticate the user with the persistent token of the local storage.
   *
   * We don't check if the token is expired or not in the page change, because is generate an unecessary overhead.
   * If the token is expired when the user try to call the first api to get data, him will be logoff and redirect
   * to login page.
   *
   * @param $rootScope
   * @param $state
   * @param $stateParams
   * @param Auth
   */
  /** @ngInject */
  function authenticationListener($rootScope, $state, Global, Auth) {

    // $stateChangeStart is fired whenever the state changes. We can use some parameters
    // such as toState to hook into details about the state as it is changing
    $rootScope.$on('$stateChangeStart', function(event, toState) {
      var authenticated = Auth.authenticated();

      //can have a token in localstorage and the flag is not defined in memory because they reopen the browser
      if (!authenticated) {
        var user = angular.fromJson(localStorage.getItem('user'));

        if (user) {
          Auth.updateCurrentUser(user);
          authenticated = true;
        }
      }

      // If there is any user data in local storage then the user is quite
      // likely authenticated. If their token is expired, or if they are
      // otherwise not actually authenticated, they will be redirected to
      // the auth state because of the rejected request anyway
      if (authenticated) {
        // If the user is logged in and we hit the auth route we don't need
        // to stay there and can send the user to the main state
        if (toState.name === Global.loginState) {
          $state.go(Global.homeState);
          event.preventDefault();
        }
      } else {
        if (toState.data.needAuthentication !== false) {  
          $state.go(Global.loginState);
          event.preventDefault();
        }
      }
    });
  }
}());
