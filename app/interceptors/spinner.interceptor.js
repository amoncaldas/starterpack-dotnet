(function () {
  'use strict';

  angular
    .module('app')
    .config(spinnerInterceptor);

  function spinnerInterceptor($httpProvider, $provide) {

    /**
     * Este interceptor é responsável por mostrar e esconder o
     * componente PrSpinner sempre que uma requisição ajax
     * iniciar e finalizar.
     *
     * @param {any} $q
     * @param {any} $injector
     * @returns
     */
    function showHideSpinner($q, $injector) {
      var $translate = $injector.get('$translate');

      return {
        request: function (config) {
          if (config.method === 'GET') {
            $injector.get('PrSpinner').show($translate.instant('loading'));
          } else {
            $injector.get('PrSpinner').show($translate.instant('processing'));
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

    // Define uma factory para o $httpInterceptor
    $provide.factory('showHideSpinner', showHideSpinner);

    // Adiciona a factory no array de interceptors do $http
    $httpProvider.interceptors.push('showHideSpinner');
  }
}());
