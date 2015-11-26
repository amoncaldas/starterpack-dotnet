(function() {
  'use strict';

  angular
    .module('app')
    .factory('RoleService', RoleService);

  RoleService.$inject = ['Restangular'];

  function RoleService(Restangular) {
    var service = Restangular.service('/roles');

    return service;
  }

}());
