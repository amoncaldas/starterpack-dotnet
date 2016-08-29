(function() {

  'use strict';

  angular
    .module('app')
    .controller('UsersDialogController', UsersDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function UsersDialogController($controller, UserService, userDialogInput) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.beforeSearch = beforeSearch;
    vm.transferUser = userDialogInput.transferUserFn;

    // instantiate base controller
    $controller('CRUDController', {
      vm: vm,
      modelService: UserService,
      options: {
        perPage: 5
      }
    });

    function onActivate() {
      vm.queryFilters = {};
    }

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, vm.queryFilters);
    }

  }

})();
