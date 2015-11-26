(function() {
  'use strict';

  angular
    .module('app')
    .factory('UserService', UserService);

  UserService.$inject = ['Restangular'];

  function UserService(Restangular) {
    var service = Restangular.service('/users');

    service.updateProfile = function(data) {
      return Restangular.one('profile').customPUT(data);
    }
    return service;
  }

}());