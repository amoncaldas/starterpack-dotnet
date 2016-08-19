(function() {
  'use strict';

  angular
    .module('app')
    .factory('TaskDialogService', TaskDialogService);

  /** @ngInject */
  function TaskDialogService(serviceFactory, Global) {
    var model = serviceFactory('tasks', {
      actions: {
        /**
         * Atualiza os status da tarefa
         *
         * @param {object} attributes
         * @returns {promise} Uma promise com o resultado da chamada no backend
         */
        toggleDone: {
          method: 'PUT',
          url: Global.apiVersion + '/tasks/toggleDone',
          override: true,
          wrap: false
        }
      },
      instance: { }
    });

    return model;
  }

}());
