(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  /** @ngInject */
  function config($authProvider, Global) {
    // Satellizer configuration that specifies which API
    // route the JWT should be retrieved from
    $authProvider.loginUrl = Global.apiVersion + '/authenticate';
    /*$injector('$mdThemingProvider').theme('default')
      .primary('blue')
      .accentPalette('orange');*/


  }
}());
