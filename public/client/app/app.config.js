(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  /** @ngInject */
  function config($authProvider, Global, $mdThemingProvider) {
    // Satellizer configuration that specifies which API
    // route the JWT should be retrieved from
    $authProvider.loginUrl = Global.apiVersion + '/authenticate';
    $mdThemingProvider.theme('default')
      .primaryPalette('blue', {
        default: '700'
      })
      .accentPalette('red')
      .warnPalette('deep-orange');

  }
}());
