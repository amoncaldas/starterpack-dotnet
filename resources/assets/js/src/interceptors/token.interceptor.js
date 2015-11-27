(function () {
  'use strict';

  angular
    .module('app')
    .config(tokenInterceptor)
    .run(stateChangeListener);

  tokenInterceptor.$inject = ['$httpProvider', '$provide', 'Global'];

  /**
   * Intercept all response (success or error) to verify the returned token
   *
   * @param $httpProvider
   * @param $provide
   * @param Global
   */
  function tokenInterceptor($httpProvider, $provide, Global) {

    function redirectWhenServerLoggedOut($q, $injector) {

      return {
        response: function (response) {
          // get a new refresh token to use in the next request
          var token = response.headers('Authorization');

          if ('undefined' !== typeof token && null !== token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }
          return response;
        },
        responseError: function (response) {
          // Instead of checking for a status code of 400 which might be used
          // for other reasons in Laravel, we check for the specific rejection
          // reasons to tell us if we need to redirect to the login state
          var rejectionReasons = ['token_not_provided', 'token_expired', 'token_absent', 'token_invalid'];

          angular.forEach(rejectionReasons, function (value, key) {
            if (response.data && response.data.error === value) {
              $injector.get('Auth').logout().then(function () {
                $injector.get('Toast').warning("Você foi deslogado do sistema por inatividade. Favor entrar no sistema novamente");

                var $state = $injector.get('$state');

                //in case multiple ajax request fail at same time because token problems, only the first will redirect
                if(!$state.is('Global.loginState')) { $state.go(Global.loginState); }
              });
            }
          });

          //many servers errors (business) are intercept here but generated a new refresh token and need update current token
          var token = response.headers('Authorization');

          if ('undefined' !== typeof token && null !== token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }

          return $q.reject(response);
        }
      };
    }

    // Setup for the $httpInterceptor
    $provide.factory('redirectWhenServerLoggedOut', redirectWhenServerLoggedOut);

    // Push the new factory onto the $http interceptor array
    $httpProvider.interceptors.push('redirectWhenServerLoggedOut');
  }

  stateChangeListener.$inject = ['$rootScope', '$state', 'Global', 'Auth'];

  /**
   * Listen all state (page) changes. Every time a state change need to verify the user is authenticated or not to redirect to correct page
   * When a user close the browser without logout, when him reopen the browser this event reauthenticate the user with the persistent token of the
   * local storage.
   * We don't check if the token is expired or not in the page change, because is generate an unecessary overhead. If the token is expired when the user
   * try to call the first api to get data, him will be logoff and redirect to login page.
   *
   * @param $rootScope
   * @param $state
   * @param $stateParams
   * @param Auth
   */
  function stateChangeListener($rootScope, $state, Global, Auth) {

    // $stateChangeStart is fired whenever the state changes. We can use some parameters
    // such as toState to hook into details about the state as it is changing
    $rootScope.$on('$stateChangeStart', function(event, toState) {
      var authenticated = Auth.authenticated();

      //can have a token in localstorage and the flag is not defined in memory because they reopen the browser
      if(!authenticated) {
        var user = JSON.parse(localStorage.getItem('user'));

        if(user) {
          Auth.updateCurrentUser(user);
          authenticated = true;
        }
      }

      // If there is any user data in local storage then the user is quite
      // likely authenticated. If their token is expired, or if they are
      // otherwise not actually authenticated, they will be redirected to
      // the auth state because of the rejected request anyway
      if(authenticated) {
        // If the user is logged in and we hit the auth route we don't need
        // to stay there and can send the user to the main state
        if(toState.name === Global.loginState) {
          $state.go(Global.homeState);
          event.preventDefault();
        }
      } else {
        if(toState.name !== Global.loginState) {
          $state.go(Global.loginState);
          event.preventDefault();
        }
      }
    });
  }
}());
