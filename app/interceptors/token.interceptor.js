(function() {
  'use strict';

  angular
    .module('app')
    .config(tokenInterceptor);

  /**
   * Intercept all response (success or error) to verify the returned token
   *
   * @param $httpProvider
   * @param $provide
   * @param Global
   */
  /** @ngInject */
  function tokenInterceptor($httpProvider, $provide, Global) {

    function redirectWhenServerLoggedOut($q, $injector) {
      var $translate = $injector.get('$translate');

      return {
        response: function(response) {
          // get a new refresh token to use in the next request
          var token = response.headers('Authorization');

          if (token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }
          return response;
        },
        responseError: function(rejection) {
          // Instead of checking for a status code of 400 which might be used
          // for other reasons in Laravel, we check for the specific rejection
          // reasons to tell us if we need to redirect to the login state
          var rejectionReasons = ['token_not_provided', 'token_expired', 'token_absent', 'token_invalid'];

          var tokenError = false;

          angular.forEach(rejectionReasons, function(value) {
            if (rejection.data && rejection.data.error === value) {
              tokenError = true;

              $injector.get('Auth').logout().then(function() {
                var $state = $injector.get('$state');

                // in case multiple ajax request fail at same time because token problems,
                // only the first will redirect and notified
                if (!$state.is(Global.loginState)) {
                  $state.go(Global.loginState);

                  $injector.get('PrToast').warn($translate.instant('messages.login.logoutInactive'));
                  //close any dialog that is opened
                  $injector.get('PrDialog').close();

                  event.preventDefault();
                }
              });
            }
          });

          //define data to empty because already show PrToast token message
          if (tokenError) {
            rejection.data = {};
          }

          // many servers errors (business) are intercept here but generated a new refresh token
          // and need update current token
          var token = rejection.headers('Authorization');

          if (token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }

          return $q.reject(rejection);
        }
      };
    }

    // Setup for the $httpInterceptor
    $provide.factory('redirectWhenServerLoggedOut', redirectWhenServerLoggedOut);

    // Push the new factory onto the $http interceptor array
    $httpProvider.interceptors.push('redirectWhenServerLoggedOut');
  }

}());
