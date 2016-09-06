(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  /**
   * Arquivo de configuração com as rotas específicas do recurso user
   *
   * @param {any} $stateProvider
   * @param {any} Global
   */
  /** @ngInject */
  function routes($stateProvider, Global) {
    $stateProvider
      .state('user', {
        url: '/usuario',
        templateUrl: Global.clientPath + '/users/users.html',
        controller: 'UsersController as usersCtrl',
        data: { needProfile: ['admin'] }
      })
      .state('user-profile', {
        url: '/usuario/perfil',
        templateUrl: Global.clientPath + '/users/profile.html',
        controller: 'ProfileController as profileCtrl',
        data: { }
      });

  }
}());
