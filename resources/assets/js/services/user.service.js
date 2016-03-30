(function() {
  'use strict';

  angular
    .module('app')
    .factory('UserService', UserService);

  UserService.$inject = ['$resource', '$q', '$http', 'Global'];

  function UserService($resource, $q, $http, Global) {
    var service = $resource(Global.apiVersion + '/users/:id', {id: '@id'},
    {
        'update': { method:'PUT' }
    });
    service.updateProfile = updateProfile;

    function updateProfile(data) {
      var deferred = $q.defer();

      $http.put(Global.apiVersion + '/profile', data).then(function(response) {
        deferred.resolve(response);
      }, function(error) {
        deferred.reject(error);
      });

      return deferred.promise;
    }
    return service;
  }

}());
