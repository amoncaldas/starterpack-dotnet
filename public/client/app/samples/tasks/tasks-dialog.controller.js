(function() {

  'use strict';

  angular
    .module('app')
    .controller('TasksDialogController', TasksDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function TasksDialogController($controller, TasksDialogService, projectId, PrToast, PrDialog, $translate) {
    var vm = this;

    //Functions Block
    vm.onActivate   = onActivate;
    vm.closeModal   = closeModal;
    vm.applyFilters = applyFilters;
    vm.beforeSave   = beforeSave;
    vm.afterSave    = afterSave;
    vm.toggleDone   = toggleDone;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: TasksDialogService, options: {
      redirectAfterSave: false,
      perPage: 5
    } });

    function onActivate() {
      vm.now  = new Date();
      vm.queryFilters = { projectId: projectId };
    }

    function applyFilters(defaultQueryFilters) {
      return angular.extend(defaultQueryFilters, vm.queryFilters);
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
      TasksDialogService.toggleDone({ id: resource.id, done: resource.done }).then(function() {
        PrToast.success($translate.instant('task.toggleDoneSuccess'));
        vm.search(vm.paginator.currentPage);
      }, function(error) {
        PrToast.errorValidation(error.data, $translate.instant('task.toggleDoneError'));
      });
    }

  }

})();
