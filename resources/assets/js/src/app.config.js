(function() {
  'use strict';

  angular
    .module('app')
    .config(config);

  config.$inject = ['$authProvider', '$httpProvider', '$provide'];

  function config($authProvider, $httpProvider, $provide) {
    // Satellizer configuration that specifies which API
    // route the JWT should be retrieved from
    $authProvider.loginUrl = '/api/authenticate';

    function redirectWhenServerLoggedOut($q, $injector) {

      return {
        response: function(response) {
          // get a new refresh token to use in the next request
          var token = response.headers('Authorization');

          if ('undefined' !== typeof token && null !== token) {
            $injector.get('$auth').setToken(token.split(' ')[1]);
          }
          return response;
        },
        responseError: function(rejection) {
          // Instead of checking for a status code of 400 which might be used
          // for other reasons in Laravel, we check for the specific rejection
          // reasons to tell us if we need to redirect to the login state
          var rejectionReasons = ['token_not_provided', 'token_expired', 'token_absent', 'token_invalid'];

          angular.forEach(rejectionReasons, function(value, key) {
            if (rejection.data.error === value) {
              $injector.get('Auth').logout().then(function() {
                toastr.info("Você foi deslogado do sistema por inatividade. Favor entrar no sistema novamente");
                $injector.get('$state').go($injector.get('GlobalService').loginState);
              });
            }
          });

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
