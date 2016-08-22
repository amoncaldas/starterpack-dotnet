(function() {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDatepicker', datepickerService);

  /** @ngInject */
  function datepickerService() {
    return {
      show: show,
      hide: hide
    };

    /**
     * Exibe o spinner com a mensagem informada
     *
     * @param {string} message - mensagem a ser exibida
     */
    function show(message) {
      //emite o sinal para a diretiva informando que o componente spinner deve ser exibido
      $rootScope.$broadcast('show-spinner', { message: message });
    }

    function hide() {
      $rootScope.$broadcast('hide-spinner');
    }
  }

})();
