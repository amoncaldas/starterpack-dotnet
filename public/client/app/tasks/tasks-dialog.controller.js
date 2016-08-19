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
    vm.projectId = projectCtrl.currentProjectId;
    vm.now  = new Date();

    //Functions Block
    vm.closeModal   = closeModal;
    vm.beforeSearch = beforeSearch;
    vm.beforeSave   = beforeSave;
    vm.afterSave    = afterSave;
    vm.toggleDone   = toggleDone;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: TaskDialogService, options: {
      redirectAfterSave: false,
      searchOnInit: projectCtrl.searchOnInit
    } });

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, { projectId: vm.projectId });
    }

    function beforeSave() {
      vm.resource.project_id = vm.projectId;
    }

    function afterSave() {
      vm.cleanForm();
    }

    function closeModal() {
      vm.cleanForm();
      $mdDialog.cancel();
    }

    function toggleDone(resource) {
      var task = {
        id: resource.id,
        done: resource.done
      };

      TaskDialogService.toggleDone(task).then(function() {
        vm.search(vm.paginator.currentPage);
      }, function(error) {
        PrToast.errorValidation(error.data, 'Não foi possível atualizar sua tarefa.');
      });
    }

  }

})();
