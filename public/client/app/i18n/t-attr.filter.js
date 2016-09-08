(function() {

  'use strict';

  angular
    .module('app')
    .filter('tAttr', tAttr);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function tAttr($filter) {
    return function(id) {
      var key = 'attributes.' + id;
      var translate = $filter('translate')(key);

      return (translate === key) ? id : translate;
    }
  }

})();
