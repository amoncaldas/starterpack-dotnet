(function() {

  'use strict';

  angular
    .module('app')
    .filter('tAttr', tAttr);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function tAttr($filter) {
    return function(id) {
      return $filter('translate')('attributes.' + id);
    }
  }

})();
