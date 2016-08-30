(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function config($authProvider, Global, $mdThemingProvider, $modelFactoryProvider,
    $translateProvider, $mdpDatePickerProvider, $mdpTimePickerProvider, moment, $translate) {

    $translateProvider
      .preferredLanguage('pt-BR')
      .useLoader('languageLoader')
      .useSanitizeValueStrategy('escape');

    moment.locale('pt-BR');

    $modelFactoryProvider.defaultOptions.prefix = Global.apiVersion;

    $mdpDatePickerProvider.setCancelButtonLabel($translate.instant('form.cancel'));
    $mdpTimePickerProvider.setCancelButtonLabel($translate.instant('form.cancel'));

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
