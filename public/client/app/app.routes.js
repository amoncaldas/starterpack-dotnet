(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  routes.$inject = ['$stateProvider', '$urlRouterProvider', 'Global'];

  function routes($stateProvider, $urlRouterProvider, Global) {
    $stateProvider
      .state('not-authorized', {
        url: '/acesso-negado',
        templateUrl: Global.clientPath + '/layout/404.html',
        data: { needAuthentication: false }
      });

    $urlRouterProvider.otherwise(Global.loginState);
  }
}());
