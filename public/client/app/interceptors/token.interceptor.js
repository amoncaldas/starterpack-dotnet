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

    function redirectWhenServerLoggedOut($q, $injector, $translate) {

      return {
        response: function(response) {
          // get a new refresh token to use in the next request
          var token = response.headers('Authorization');

          if (token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }
          return response;
        },
        responseError: function(response) {
          // Instead of checking for a status code of 400 which might be used
          // for other reasons in Laravel, we check for the specific rejection
          // reasons to tell us if we need to redirect to the login state
          var rejectionReasons = ['token_not_provided', 'token_expired', 'token_absent', 'token_invalid'];

          angular.forEach(rejectionReasons, function(value) {
            if (response.data && response.data.error === value) {
              $injector.get('Auth').logout().then(function() {
                var $state = $injector.get('$state');

                // in case multiple ajax request fail at same time because token problems,
                // only the first will redirect and notified
                if (!$state.is(Global.loginState)) {
                  $state.go(Global.loginState);
                  //close any dialog that is opened
                  $injector.get('PrDialog').close();

                  $injector.get('PrToast')
                    .warn($translate.instant('messages.logoutInactive'));

                  event.preventDefault();
                }
              });
            }
          });

          // many servers errors (business) are intercept here but generated a new refresh token
          // and need update current token
          var token = response.headers('Authorization');

          if (token) {
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

}());
