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
      instance: { }
    });

    return model;
  }

}());
