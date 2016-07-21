/* eslint angular/file-name: 0 */
(function() {
  'use strict';

  angular
    .module('app')
    .factory('UserModel', UserModel);

  /** @ngInject */
  function UserModel(lodash) {
    var User = function() {
      this.roles = [];
    }

    User.prototype.hasProfile = function(profile) {
      return lodash.includes(this.roles, profile);
    }

    User.prototype.isAdmin = function() {
      return this.hasProfile('admin');
    }

    return {
      getInstanceWithAttributes: function(attributes) {
        var user = new User();

        user = lodash.assign(user, attributes)

        return user;
      }
    };
  }

}());
