(function() {
  'use strict';

  angular
    .module('app')
    .factory('<%= name_uppercase %>Service', <%= name_uppercase %>Service);

  /** @ngInject */
  function <%= name_uppercase %>Service(serviceFactory) {
    var model = serviceFactory('<%= resource_name %>', {
      actions: { },
      instance: { }
    });

    return model;
  }

}());
