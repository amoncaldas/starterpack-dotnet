(function () {
  'use strict';

  angular
    .module('app')
    .config(spinnerInterceptor);

  spinnerInterceptor.$inject = ['$httpProvider', '$provide'];

  /**
   * Intercept all response (success or error) to verify the returned token
   *
   * @param $httpProvider
   * @param $provide
   * @param Global
   */
  function spinnerInterceptor($httpProvider, $provide) {

    function showHideSpinner($q, $injector) {

      return {
        request: function (config) {
          if(config.method === "GET")
            $injector.get('Spinner').show('Carregando...');
          else
            $injector.get('Spinner').show('Processando...');

          return config;
        },
        response: function (response) {
          $injector.get('Spinner').hide();

          return response;
        }
      };
    }

    // Setup for the $httpInterceptor
    $provide.factory('showHideSpinner', showHideSpinner);

    // Push the new factory onto the $http interceptor array
    $httpProvider.interceptors.push('showHideSpinner');
  }
}());