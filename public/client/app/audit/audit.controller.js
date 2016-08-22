(function() {

  'use strict';

  angular
    .module('app')
    .controller('AuditController', AuditController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditController($controller, AuditService, PrDialog) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.beforeSearch = beforeSearch;
    vm.viewDetail = viewDetail;

    $controller('CRUDController', { vm: vm, modelService: AuditService, options: {} });

    function onActivate() {
      vm.models = AuditService.listModels();
      vm.types = AuditService.listTypes();

      vm.queryFilters = { type: vm.types[0].id, model: vm.models[0].id };
    }

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, vm.queryFilters);
    }

    function viewDetail(auditDetail) {
      var options = {
        locals: { auditDetail: auditDetail },
        /** @ngInject */
        controller: function(auditDetail) {
          var vm = this;

          vm.auditDetail = auditDetail;
        },
        controllerAs: 'auditDetailCtrl',
        templateUrl: '/audit/audit-detail.html'
      };

      PrDialog.show('custom', options);
    }


  }

})();
