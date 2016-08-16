(function() {
  'use strict';

  angular
    .module('app')
    .factory('ProjectService', ProjectService);

  /** @ngInject */
  function ProjectService(serviceFactory) {
    var model = serviceFactory('projects', {
      actions: { },
      instance: { }
    });

    return model;
  }

}());
