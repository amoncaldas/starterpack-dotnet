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
          return $translate.instant('audit.dialog.title.created');
        case 'updated':
          if (status === 'before') {
            return $translate.instant('audit.dialog.title.updatedBefore');
          } else {
            return $translate.instant('audit.dialog.title.updatedAfter');
          }
        case 'deleted': return $translate.instant('audit.dialog.title.deleted');
      }
    }
  }

})();
