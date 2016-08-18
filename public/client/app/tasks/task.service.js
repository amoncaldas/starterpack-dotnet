(function() {
  'use strict';

  angular
    .module('app')
    .factory('TaskService', TaskService);

  /** @ngInject */
  function TaskService(serviceFactory) {
    var model = serviceFactory('tasks', {
      actions: { },
      instance: { }
    });

    return model;
  }

}());
