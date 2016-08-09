(function() {
  'use strict';

  angular
    .module('app')
    .factory('RoleService', RoleService);

  /** @ngInject */
  function RoleService(serviceFactory) {
    var model = serviceFactory('roles');

    return model;
  }

}());
