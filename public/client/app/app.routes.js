(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  routes.$inject = ['$stateProvider', '$urlRouterProvider', 'Global'];

  function routes($stateProvider, $urlRouterProvider, Global) {
    $stateProvider
      .state(Global.homeState, {
        url: '/',
        templateUrl: Global.clientPath + '/dashboard/dashboard.html',
        controller: 'DashboardController as dashboardCtrl'
      })
      .state(Global.loginState, {
        url: '/login',
        templateUrl: Global.clientPath + '/auth/login.html',
        controller: 'LoginController as loginCtrl'
      })
      .state('not-authorized', {
        url: '/acesso-negado',
        templateUrl: Global.clientPath + '/layout/404.html'
      });

    $urlRouterProvider.otherwise(Global.loginState);
  }
}());
