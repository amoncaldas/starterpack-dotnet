(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function config($authProvider, Global, $mdThemingProvider, $modelFactoryProvider,
    $translateProvider) {

    $translateProvider
      .preferredLanguage('pt-BR')
      .useLoader('languageLoader')
      .useSanitizeValueStrategy('escape');

    $modelFactoryProvider.defaultOptions.prefix = Global.apiVersion;

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
