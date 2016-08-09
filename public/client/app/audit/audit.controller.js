(function() {

  'use strict';

  angular
    .module('app')
    .controller('AuditController', AuditController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditController(PrToast, PrPagination, AuditService) {
    var vm = this;

    vm.search = search;

    activate();

    function activate() {
      vm.viewForm = false;
      vm.params = {};

      vm.paginator = PrPagination.getInstance(search, 10);
      vm.search(1);
    }

    function search(page) {
      vm.paginator.currentPage = page;

      AuditService.paginate({ page: page, perPage: vm.paginator.perPage }).then(function (response) {
        vm.paginator.calcNumberOfPages(response.total);
        vm.logs = response.items;
        console.log(vm.logs)
      }, function () {
        PrToast.error('Não foi possível realizar a busca de usuários');
      });
    }

  }

})();
