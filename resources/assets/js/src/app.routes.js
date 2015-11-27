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
        templateUrl: 'templates/home.html',
        controller: 'HomeController as home'
      })
      .state(Global.loginState, {
        url: '/login',
        templateUrl: 'templates/auth/loginForm.html',
        controller: 'AuthController as auth'
      })
      .state('usuarios', {
        url: '/usuarios',
        templateUrl: 'templates/users/index.html',
        controller: 'UsersController as usersCtrl',
        data: { needAdmin: true }
      })
      .state('profile', {
        url: '/profile',
        templateUrl: 'templates/users/profile.html',
        controller: 'ProfileController as profileCtrl'
      })
      .state('not-authorized', {
        url: '/acesso-negado',
        templateUrl: 'templates/404.html'
      });

    $urlRouterProvider.otherwise(Global.loginState);
  }
}());
