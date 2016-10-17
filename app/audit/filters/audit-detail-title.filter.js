(function() {

  'use strict';

  angular
    .module('app')
    .filter('auditDetailTitle', auditDetailTitle);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function auditDetailTitle($translate) {
    return function(auditDetail, status) {
      switch (auditDetail.type) {
        case 'created':
          return $translate.instant('dialog.audit.created');
        case 'updated':
          if (status === 'before') {
            return $translate.instant('dialog.audit.updatedBefore');
          } else {
            return $translate.instant('dialog.audit.updatedAfter');
          }
        case 'deleted': return $translate.instant('dialog.audit.deleted');
      }
    }
  }

})();
