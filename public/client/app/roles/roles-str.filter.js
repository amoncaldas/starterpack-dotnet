(function() {

  'use strict';

  angular
    .module('app')
    .filter('rolesStr', rolesStr);

  /** @ngInject */
  function rolesStr(lodash) {
    return function(roles) {
      return lodash.map(roles, 'slug').join(', ');
    };
  }

})();
