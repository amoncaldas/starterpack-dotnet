(function() {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrModal', modalService);

  /** @ngInject */
  function modalService() {
    /*return {
      alert: alert,
      confirm: confirm,
      prompt: prompt
      //custom: custom
    };*/

    /*var PrModal = function(controller, targetEvent, options) {

      var defaultOptions = {

      };

      angular.merge(defaultOptions, options);

      $mdDialog.show()
      .then(function(result) {

      });
    }*/

    /**
     * Exibe o spinner com a mensagem informada
     *
     * @param {string} message - mensagem a ser exibida

    function alert(message) {
      //emite o sinal para a diretiva informando que o componente spinner deve ser exibido
      $rootScope.$broadcast('show-spinner', { message: message });
    }

    function alert() {
      $rootScope.$broadcast('hide-spinner');
    }*/
  }

})();
