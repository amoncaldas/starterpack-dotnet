(function() {
  'use strict';

  angular
    .module('app')
    .factory('AuditService', AuditService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditService(serviceFactory, $translate) {
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
          { id: '', label: $translate.instant('views.fields.audit.allResources') },
          { id: 'created', label: $translate.instant('views.fields.audit.type.created') },
          { id: 'updated', label: $translate.instant('views.fields.audit.type.updated') },
          { id: 'deleted', label: $translate.instant('views.fields.audit.type.deleted') }
        ];
      }
    });

    return model;
  }

}());
