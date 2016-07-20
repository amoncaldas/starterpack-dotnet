(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  /** @ngInject */
  function routes($stateProvider, Global) {
    $stateProvider
      .state('user', {
        url: '/usuario',
        templateUrl: Global.clientPath + '/users/users.html',
        controller: 'UsersController as usersCtrl',
        data: { needAdmin: true }
      })
      .state('user-profile', {
        url: '/usuario/perfil',
        templateUrl: Global.clientPath + '/users/profile.html',
        controller: 'ProfileController as profileCtrl'
      });

  }
}());
