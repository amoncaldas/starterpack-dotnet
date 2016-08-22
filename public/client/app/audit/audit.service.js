(function() {
  'use strict';

  angular
    .module('app')
    .factory('AuditService', AuditService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditService(serviceFactory) {
    var model = serviceFactory('audit', {
      actions: { },
      instance: { },
      listTypes: function() {
        return [
          { id: '', label: 'Todos' },
          { id: 'created', label: 'Cadastrado' },
          { id: 'updated', label: 'Atualizado' },
          { id: 'deleted', label: 'Removido' }
        ];
      },

      listModels: function() {
        return [
          { id: '', label: 'Todos Recursos' },
          { id: 'Project', label: 'Projeto' },
          { id: 'Task', label: 'Tarefa' },
          { id: 'User', label: 'Usuário' }
        ];
      }
    });

    return model;
  }

}());
