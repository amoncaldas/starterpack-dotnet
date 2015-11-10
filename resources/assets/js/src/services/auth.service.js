(function() {
  'use strict';

  angular
    .module('app')
    .factory('Auth', Auth);

  Auth.$inject = ['$http', '$auth', '$q', '$rootScope'];

  function Auth($http, $auth, $q, $rootScope) {
    return {
      login: login,
      logout: logout
    };

    function login(credentials) {
      var deferred = $q.defer();

      $auth.login(credentials)
        .then(function(data) {
          return $http.get('api/authenticate/user');
        })
        .then(function(response) {
          var user = JSON.stringify(response.data.user);

          localStorage.setItem('user', user);

          $rootScope.authenticated = true;
          $rootScope.currentUser = response.data.user;

          deferred.resolve();
        }, function(error) {
          deferred.reject(error);
        });

      return deferred.promise;
    }

    function logout() {
      var deferred = $q.defer();

      $auth.logout().then(function() {
        localStorage.removeItem('user');

        $rootScope.authenticated = false;
        $rootScope.currentUser = null;

        deferred.resolve();
      });

      return deferred.promise;
    }
  }

}());
