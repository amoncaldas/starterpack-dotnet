(function() {

  'use strict';

  angular
    .module('app')
    .controller('AuditController', AuditController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function AuditController($controller, AuditService) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.beforeSearch = beforeSearch;
    vm.afterSearch = afterSearch;

    $controller('CRUDController', { vm: vm, modelService: AuditService, options: {} });

    function onActivate() {
      vm.models = [
        { id: '', label: 'Todos Recursos' },
        { id: 'Project', label: 'Projeto' },
        { id: 'Task', label: 'Tarefa' },
        { id: 'User', label: 'Usuário' }
      ];

      vm.types = [
        { id: '', label: 'Todos' },
        { id: 'created', label: 'Cadastrado' },
        { id: 'updated', label: 'Atualizado' },
        { id: 'deleted', label: 'Removido' }
      ]

      vm.queryFilters = { type: vm.types[0].id, model: vm.models[0].id };
    }

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, vm.queryFilters);
    }

    function afterSearch() {
      var items = vm.resources;

      for (var i = 0; i < items.length; i++) {
        items[i].updated_at = new Date(items[i].updated_at);
      }

      vm.resources = items;
    }


  }

})();
