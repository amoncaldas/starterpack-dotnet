(function() {
  'use strict';

  angular
    .module('app')
    .factory('Auth', Auth);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function Auth($http, $auth, $q, Global, lodash) {
    var auth = {
      login: login,
      logout: logout,
      updateCurrentUser: updateCurrentUser,
      retrieveUserFromLocalStorage: retrieveUserFromLocalStorage,
      authenticated: authenticated,
      is: is,
      isAdmin: isAdmin,
      currentUser: null
    };

    function is(profile) {
      return (angular.isObject(auth.currentUser) && lodash.includes(auth.currentUser.roles, profile));
    }

    function isAdmin() {
      return auth.is('admin');
    }

    function authenticated() {
      return (auth.currentUser);
    }

    function retrieveUserFromLocalStorage(user) {
      var user = localStorage.getItem('user');

      if (user) {
        auth.currentUser = angular.fromJson(user);
      }
    }

    function updateCurrentUser(user) {
      if (user) {
        if (angular.isUndefined(user.roles)) { user.roles = []; }

        var jsonUser = angular.toJson(user);

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
        .then(function() {
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
