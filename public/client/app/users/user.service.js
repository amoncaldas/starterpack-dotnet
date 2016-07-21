(function() {
  'use strict';

  angular
    .module('app')
    .factory('UserService', UserService);

  /** @ngInject */
  function UserService($resource, $q, $http, Global) {
    //Cria um serviço para o recurso Usuário.
    //Através desse serviço é feita a comunicação com o backend.
    var service = $resource(Global.apiVersion + '/users/:id', { id: '@id' }, {
      update: { method: 'PUT' }
    });

    service.updateProfile = updateProfile;

    /**
     * Atualiza o perfil do usuário logado
     *
     * @param {any} attributes Dados do usuário para atualizar
     * @returns Uma promise com o retorno do serviço
     */
    function updateProfile(attributes) {
      var deferred = $q.defer();

      $http.put(Global.apiVersion + '/profile', attributes).then(function(response) {
        deferred.resolve(response);
      }, function(error) {
        deferred.reject(error);
      });

      return deferred.promise;
    }

    return service;
  }

}());
