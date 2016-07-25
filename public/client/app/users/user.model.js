/* eslint angular/file-name: 0 */
(function() {
  'use strict';

  angular
    .module('app')
    .factory('UserModel', UserModel);

  /**
   * Serviço que representa o model de Usuário
   * Foi criado somente para adicionar alguns métodos de negócio no lado do cliente
   * e utilizar principalmente para referência do usuário logado.
   *
   * @param {any} lodash
   * @returns
   */
  /** @ngInject */
  function UserModel(lodash) {
    /**
     * Construtor
     */
    var User = function() {
      this.roles = [];
    }

    /**
     * Verifica se o usuário tem os perfis informados.
     *
     * @param {any} profile
     * @returns {boolean}
     */
    User.prototype.hasProfile = function(roles, all) {
      roles = angular.isArray(roles) ? roles : [roles];

      if (all) {
        return lodash.intersection(this.roles, roles).length === roles.length;
      } else { //return the length because 0 is false in js
        return lodash.intersection(this.roles, roles).length;
      }
    }

    /**
     * Verifica se o usuário é admin.
     *
     * @returns {boolean}
     */
    User.prototype.isAdmin = function() {
      return this.hasProfile('admin');
    }

    return {
      /**
       * Crie e retorna uma instancia do usuário com base nos
       * atributos informados
       *
       * @param {any} attributes
       * @returns
       */
      getInstanceWithAttributes: function(attributes) {
        var user = new User();

        //faz o merge dos atributos informados com a instancia criada
        user = lodash.assign(user, attributes)

        return user;
      }
    };
  }

}());
