(function() {
  'use strict';

  angular
    .module('app')
    .factory('Auth', Auth);

  Auth.$inject = ['$http', '$auth', '$q', '$rootScope', 'Global', 'lodash'];

  function Auth($http, $auth, $q, $rootScope, Global, _) {
    var auth = {
      login: login,
      logout: logout,
      updateCurrentUser: updateCurrentUser,
      authenticated: authenticated,
      is: is,
      isAdmin: isAdmin,
      currentUser: null
    };

    function is(profile) {
      return (angular.isObject(auth.currentUser) && _.includes(auth.currentUser.roles, profile) );
    }

    function isAdmin() {
      return auth.is('admin');
    }

    function authenticated() {
      return (auth.currentUser);
    }

    function updateCurrentUser(user) {
      if(user !== null) {
        if(angular.isUndefined(user.roles)) { user.roles = []; }

        var jsonUser = JSON.stringify(user);

        localStorage.setItem('user', jsonUser);
        auth.currentUser = user;
      } else {
        localStorage.removeItem('user');
        auth.currentUser = null;
      }
    }

    function login(credentials) {
      var deferred = $q.defer();

      $auth.login(credentials)
        .then(function(data) {
          return $http.get(Global.apiVersion + '/authenticate/user');
        })
        .then(function(response) {
          auth.updateCurrentUser(response.data.user);

          deferred.resolve();
        }, function(error) {
          deferred.reject(error);
        });

      return deferred.promise;
    }

    function logout() {
      var deferred = $q.defer();

      $auth.logout().then(function() {
        auth.updateCurrentUser(null);

        deferred.resolve();
      });

      return deferred.promise;
    }

    return auth;
  }

}());
