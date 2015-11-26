(function() {

  'use strict';

  angular
    .module('app')
    .filter('rolesStr', rolesStr);

  rolesStr.$inject = ['lodash'];

  function rolesStr(_) {
    return function(roles) {
      return _.pluck(roles, 'slug').join(", ");
    };
  }

})();
