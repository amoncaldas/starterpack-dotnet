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
        controller: 'DashboardController as dashboardCtrl',
        data: { breadcrumbs: 'Dashboard' }
      })
      .state(Global.loginState, {
        url: '/login',
        templateUrl: Global.clientPath + '/auth/login.html',
        controller: 'LoginController as loginCtrl'
      })
      .state('not-authorized', {
        url: '/acesso-negado',
        templateUrl: Global.clientPath + '/layout/404.html',
        data: { breadcrumbs: 'Acesso Negado' }
      });

    $urlRouterProvider.otherwise(Global.loginState);
  }
}());
