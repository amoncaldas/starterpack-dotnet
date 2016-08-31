(function() {
  'use strict';

  angular
    .module('app')
    .factory('UsersService', UsersService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function UsersService(lodash, Global, serviceFactory) {
    var model = serviceFactory('users', {
      // only called on empty inits
      defaults: {
        roles: []
      },

      actions: {
        /**
         * Atualiza os dados do perfil do usuário logado
         *
         * @param {object} attributes
         * @returns {promise} Uma promise com o resultado do chamada no backend
         */
        updateProfile: {
          method: 'PUT',
          url: Global.apiVersion + '/profile',
          override: true,
          wrap: false
        }
      },

      instance: {
        /**
         * Verifica se o usuário tem os perfis informados.
         *
         * @param {any} profile
         * @returns {boolean}
         */
        hasProfile: function(roles, all) {
          roles = angular.isArray(roles) ? roles : [roles];

          if (all) {
            return lodash.intersection(this.roles, roles).length === roles.length;
          } else { //return the length because 0 is false in js
            return lodash.intersection(this.roles, roles).length;
          }
        },

        /**
         * Verifica se o usuário é admin.
         *
         * @returns {boolean}
         */
        isAdmin: function() {
          return this.hasProfile('admin');
        }
      }
    });

    return model;
  }

}());
