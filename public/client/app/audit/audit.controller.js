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
      vm.queryFilters = {};

      vm.resources = [
        { id: 'Project', label: 'Projeto' },
        { id: 'Task', label: 'Tarefa' },
        { id: 'User', label: 'Usuário' }
      ];

      vm.types = [
        { id: null, label: 'Todos' },
        { id: 'created', label: 'Cadastrado' },
        { id: 'updated', label: 'Atualizado' },
        { id: 'deleted', label: 'Removido' }
      ]

      vm.paginator = PrPagination.getInstance(search, 10);
      vm.search(1);
    }

    function search(page) {
      vm.paginator.currentPage = page;

      var filters = angular.extend({}, { page: page, perPage: vm.paginator.perPage }, vm.queryFilters);

      AuditService.paginate(filters).then(function (response) {
        vm.paginator.calcNumberOfPages(response.total);
        vm.logs = response.items;
      }, function () {
        PrToast.error('Não foi possível realizar a busca de usuários');
      });
    }

  }

})();
