(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  routes.$inject = ['$stateProvider', '$urlRouterProvider', 'GlobalServiceProvider'];

  function routes($stateProvider, $urlRouterProvider, GlobalServiceProvider) {
    // Redirect to the auth state if any other states
    // are requested other than users
    $urlRouterProvider.otherwise(GlobalServiceProvider.$get().loginState);

    $stateProvider
      .state(GlobalServiceProvider.$get().loginState, {
        url: '/login',
        templateUrl: 'templates/auth/loginForm.html',
        controller: 'AuthController as auth'
      })
      .state(GlobalServiceProvider.$get().homeState, {
        url: '/home',
        templateUrl: 'templates/home.html',
        controller: 'HomeController as home'
      });
  }
}());
