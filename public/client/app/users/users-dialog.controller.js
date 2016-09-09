(function() {

  'use strict';

  angular
    .module('app')
    .controller('UsersDialogController', UsersDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function UsersDialogController($controller, UsersService, userDialogInput) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.applyFilters = applyFilters;
    vm.transferUser = userDialogInput.transferUserFn;

    // instantiate base controller
    $controller('CRUDController', {
      vm: vm,
      modelService: UsersService,
      options: {
        perPage: 5
      }
    });

    function onActivate() {
      vm.queryFilters = {};
    }

    function applyFilters(defaultQueryFilters) {
      return angular.extend(defaultQueryFilters, vm.queryFilters);
    }

  }

})();
