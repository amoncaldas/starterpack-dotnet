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

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: <%= name_uppercase %>Service, options: { } });

  }

})();
