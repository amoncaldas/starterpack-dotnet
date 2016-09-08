(function() {

  'use strict';

  angular
    .module('app')
    .filter('tBreadcrumb', tBreadcrumb);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function tBreadcrumb($filter) {
    return function(id) {
      var key = 'breadcrumbs.' + id;
      var translate = $filter('translate')(key);

      return (translate === key) ? id : translate;
    }
  }

})();
