(function() {
  'use strict';

  angular
    .module('app')
    .factory('RoleService', RoleService);

  RoleService.$inject = ['Global', '$resource'];

  function RoleService(Global, $resource) {
    var service = $resource(Global.apiVersion + '/roles/:id', null,
    {
        'update': { method:'PUT' }
    });

    return service;
  }

}());
