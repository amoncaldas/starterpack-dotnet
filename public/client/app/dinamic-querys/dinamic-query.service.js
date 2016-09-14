(function() {
  'use strict';

  angular
    .module('app')
    .factory('DinamicQueryService', DinamicQueryService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function DinamicQueryService(serviceFactory) {
    var model = serviceFactory('dinamicQuery', {
      actions: { 
        /**
         * ação adicionada para pegar uma lista de models existentes no servidor
         */
        getModels: {
          method: 'GET',
          url: 'models'          
        }        
      },
      instance: {
      }
    });

    return model;
  }

}());
