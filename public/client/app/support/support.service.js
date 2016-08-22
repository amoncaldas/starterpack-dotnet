(function() {
  'use strict';

  angular
    .module('app')
    .factory('SupportService', SupportService);

  /** @ngInject */
  function SupportService(serviceFactory) {
    var model = serviceFactory('support', {
      actions: {
      /**
       * Atualiza os dados do perfil do usuário logado
       *
       * @param {object} attributes
       * @returns {promise} Uma promise com o resultado do chamada no backend
       */
        langs: {
          method: 'GET',
          url: 'langs',
          wrap: false,
          cache: true
        }
      }
    });

    return model;
  }

}());
