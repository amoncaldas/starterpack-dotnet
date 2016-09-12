(function() {
  'use strict';

  angular
    .module('app')
    .factory('DinamicQueryService', DinamicQueryService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function DinamicQueryService(serviceFactory, $translate) {
    var model = serviceFactory('dinamicQuery', {
      actions: { 
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
