(function() {

  'use strict';

  angular
    .module('app')
    .controller('TasksDialogController', TasksDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function TasksDialogController($mdDialog, $controller, TaskDialogService, projectCtrl, PrToast) {
    var vm = this;

    //Attributes Block
    vm.now  = new Date();
    vm.queryFilters = { projectId: projectCtrl.currentProjectId };

    //Functions Block
    vm.closeModal   = closeModal;
    vm.beforeSearch = beforeSearch;
    vm.beforeSave   = beforeSave;
    vm.afterSave    = afterSave;
    vm.toggleDone   = toggleDone;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: TaskDialogService, options: {
      redirectAfterSave: false,
      perPage: 5
    } });

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
      $mdDialog.cancel();
    }

    function toggleDone(resource) {
      TaskDialogService.toggleDone({ id: resource.id, done: resource.done }).then(function() {
        PrToast.success('Operação realizada com sucesso.');
        vm.search(vm.paginator.currentPage);
      }, function(error) {
        PrToast.errorValidation(error.data, 'Não foi possível atualizar sua tarefa.');
      });
    }

  }

})();
