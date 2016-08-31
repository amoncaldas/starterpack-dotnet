(function() {

  'use strict';

  angular
    .module('app')
    .controller('UsersController', UsersController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function UsersController($controller, lodash, UsersService, RolesService, PrToast, Auth, PrDialog, $translate) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.afterEdit = afterEdit;
    vm.afterClean = afterClean;
    vm.beforeSave = beforeSave;
    vm.afterSave = afterSave;
    vm.beforeRemove = beforeRemove;
    vm.removeUser = removeUser;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: UsersService, options: {} });

    function onActivate() {
      vm.roles = RolesService.query().then(function (response) {
        vm.roles = response;
      });
    }

    function afterClean() {
      vm.roles.forEach(function(role) {
        role.selected = false;
      });
    }

    function afterEdit() {
      vm.roles.forEach(function(role) {
        vm.resource.roles.forEach(function(roleUser) {
          if (role.id === roleUser.id) {
            role.selected = true;
          }
        });
      });
    }

    function beforeSave() {
      //filtra o array de roles para extrair somente os ids
      vm.resource.roles = lodash.map(lodash.filter(angular.copy(vm.roles), { selected: true }), 'id');
    }

    function afterSave(resource) {
      if (vm.resource.id === Auth.currentUser.id) {
        Auth.updateCurrentUser(resource.plain());
      }
    }

    function beforeRemove(resource) {
      if (resource.id === Auth.currentUser.id) {
        PrToast.error($translate.instant('user.beforeRemoveError'));
        return false;
      };
    }

    function removeUser(resource) {
      var config = {
        title: $translate.instant('user.confirm.title'),
        description: $translate.instant('user.confirm.description', resource)
      };

      PrDialog.confirm(config).then(function() {
        vm.remove(resource)
      });
    }

  }

})();
