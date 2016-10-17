(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function config($authProvider, Global, $mdThemingProvider, $modelFactoryProvider,
    $translateProvider, $mdpDatePickerProvider, $mdpTimePickerProvider, moment) {

    $translateProvider
      .preferredLanguage('pt-BR')
      .useLoader('languageLoader')
      .useSanitizeValueStrategy('escape');

    $translateProvider.usePostCompiling(true);

    moment.locale('pt-BR');

    //os serviços referente aos models vai utilizar como base nas urls 
    $modelFactoryProvider.defaultOptions.prefix = Global.apiPath;

    $mdpDatePickerProvider.setCancelButtonLabel('Cancelar');
    $mdpTimePickerProvider.setCancelButtonLabel('Cancelar');

    // Satellizer configuration that specifies which API
    // route the JWT should be retrieved from
    $authProvider.loginUrl = Global.apiPath + '/authenticate';

    // Configuration theme
    $mdThemingProvider.theme('default')
      .primaryPalette('blue', {
        default: '700'
      })
      .accentPalette('red')
      .warnPalette('deep-orange');

  }
}());
