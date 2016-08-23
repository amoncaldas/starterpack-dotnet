(function() {

  'use strict';

  angular
    .module('app')
    .filter('auditModel', auditModel);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function auditModel(lodash, AuditService) {
    return function(modelId) {
      modelId = modelId.replace('App\\', '');
      var model = lodash.find(AuditService.listModels(), { id: modelId });

      return (model) ? model.label : model;
    }
  }

})();
