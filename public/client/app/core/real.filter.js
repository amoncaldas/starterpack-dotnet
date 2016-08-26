(function() {

  'use strict';

  angular
    .module('app')
    .filter('real', real);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function real($filter) {
    return function(value) {
      return $filter('currency')(value, 'R$ ');
    }
  }

})();
