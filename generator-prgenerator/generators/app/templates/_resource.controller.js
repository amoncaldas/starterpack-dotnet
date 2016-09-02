(function() {

  'use strict';

  angular
    .module('app')
    .controller('<%= name_uppercase %>Controller', <%= name_uppercase %>Controller);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function <%= name_uppercase %>Controller($controller, <%= name_uppercase %>Service, PrDialog, $translate) {
    var vm = this;

    //Attributes Block

    //Functions Block
    vm.afterSave = afterSave;
    vm.remove<%= name_uppercase %> = remove<%= name_uppercase %>;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: <%= name_uppercase %>Service, options: { } });

    function afterSave() {
      vm.cleanForm();
      vm.search(vm.paginator.currentPage);
    }

    function remove<%= name_uppercase %>(resource) {
      var config = {
        title: $translate.instant('dialog.confirmTitle'),
        description: $translate.instant('dialog.confirmDescription', resource)
      }

      PrDialog.confirm(config).then(function() {
        vm.remove(resource);
      });
    }

  }

})();
