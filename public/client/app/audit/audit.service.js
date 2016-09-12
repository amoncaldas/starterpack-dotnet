(function() {
  'use strict';

  angular
    .module('app')
    .factory('AuditService', AuditService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditService(serviceFactory) {
    var model = serviceFactory('audit', {
      actions: { 
        getAuditedModels: {
          method: 'GET',
          url: 'models'          
        }        
      },
      instance: {
      },
      listTypes: function() {
        return [
          { id: '', label: 'Todos' },
          { id: 'created', label: 'Cadastrado' },
          { id: 'updated', label: 'Atualizado' },
          { id: 'deleted', label: 'Removido' }
        ];
      }
    });

    return model;
  }

}());
