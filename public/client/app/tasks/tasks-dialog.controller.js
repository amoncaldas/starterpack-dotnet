(function() {

  'use strict';

  angular
    .module('app')
    .controller('TasksDialogController', TasksDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function TasksDialogController($controller, TaskDialogService, projectId, PrToast, PrDialog) {
    var vm = this;

    //Functions Block
    vm.onActivate   = onActivate;
    vm.closeModal   = closeModal;
    vm.beforeSearch = beforeSearch;
    vm.beforeSave   = beforeSave;
    vm.afterSave    = afterSave;
    vm.toggleDone   = toggleDone;
    vm.removeTask   = removeTask;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: TaskDialogService, options: {
      redirectAfterSave: false,
      perPage: 5
    } });

    function onActivate() {
      vm.now  = new Date();
      vm.queryFilters = { projectId: projectId };
    }

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, vm.queryFilters);
    }

    function beforeSave() {
      vm.resource.project_id = vm.queryFilters.projectId;
    }

    function afterSave() {
      vm.cleanForm();
      vm.search(vm.paginator.currentPage);
    }

    function closeModal() {
      vm.cleanForm();
      PrDialog.close();
    }

    function toggleDone(resource) {
      TaskDialogService.toggleDone({ id: resource.id, done: resource.done }).then(function() {
        PrToast.success('Operação realizada com sucesso.');
        vm.search(vm.paginator.currentPage);
      }, function(error) {
        PrToast.errorValidation(error.data, 'Não foi possível atualizar sua tarefa.');
      });
    }

    function removeTask(resource) {
      var config = {
        title: 'Confirmar remoção',
        description: 'Deseja remover permanentemente a tarefa selecionada?'
      }

      PrDialog.build(config)
        .confirm(config)
        .then(function() {
          vm.remove(resource);
        });
    }

  }

})();
