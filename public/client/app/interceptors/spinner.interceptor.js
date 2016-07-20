(function () {
  'use strict';

  angular
    .module('app')
    .config(spinnerInterceptor);

  /** @ngInject */
  function spinnerInterceptor($httpProvider, $provide) {

    function showHideSpinner($q, $injector) {

      return {
        request: function (config) {
          if (config.method === 'GET') {
            $injector.get('PrSpinner').show('Carregando...');
          } else {
            $injector.get('PrSpinner').show('Processando...');
          }
          return config;
        },
        response: function (response) {
          $injector.get('PrSpinner').hide();

          return response;
        },
        responseError: function (rejection) {
          $injector.get('PrSpinner').hide();

          return $q.reject(rejection);
        }
      };
    }

    // Setup for the $httpInterceptor
    $provide.factory('showHideSpinner', showHideSpinner);

    // Push the new factory onto the $http interceptor array
    $httpProvider.interceptors.push('showHideSpinner');
  }
}());
