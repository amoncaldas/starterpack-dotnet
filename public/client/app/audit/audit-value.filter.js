(function() {

  'use strict';

  angular
    .module('app')
    .filter('auditValue', auditValue);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function auditValue($filter, lodash) {
    return function(value, key) {
      if (angular.isDate(value) || lodash.endsWith(key, '_at') ||  lodash.endsWith(key, '_to')) {
        return $filter('prDatetime')(value);
      }

      if (typeof value === 'boolean') {
        return $filter('translate')((value) ? 'yes' : 'no');
      }

      return value;
    }
  }

})();
