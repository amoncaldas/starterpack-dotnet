(function() {
  'use strict';

  angular
    .module('app')
    .factory('TasksDialogService', TasksDialogService);

  /** @ngInject */
  function TasksDialogService(serviceFactory) {
    var model = serviceFactory('tasks', {
      actions: {
        /**
         * Atualiza os status da tarefa
         *
         * @param {object} attributes
         */
        toggleDone: {
          method: 'PUT',
          url: 'toggleDone'
        }
      },
      instance: { }
    });

    return model;
  }

}());
