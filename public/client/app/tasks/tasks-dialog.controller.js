(function() {

  'use strict';

  angular
    .module('app')
    .controller('TasksDialogController', TasksDialogController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function TasksDialogController($mdDialog, $controller, TaskService, projectCtrl) {
    var vm = this;

    //Attributes Block
    vm.task = {};
    vm.now  = new Date();

    //Functions Block
    vm.onActivate   = onActivate;
    vm.closeModal   = closeModal;
    vm.beforeSearch = beforeSearch;
    vm.beforeSave   = beforeSave;
    vm.afterSave    = afterSave;
    vm.cleanForm    = cleanForm;
    vm.setDone      = setDone;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: TaskService, options: {
      redirectAfterSave: false,
      searchOnInit: projectCtrl.searchOnInit
    } });

    function onActivate() {
      vm.cleanForm();
    }

    function beforeSearch() {
      angular.extend(vm.defaultQueryFilters, { projectId: projectCtrl.currentProjectId });
    }

    function beforeSave() {
      vm.resource.project = { id: projectCtrl.currentProjectId };
    }

    function afterSave() {
      vm.cleanForm();
    }

    function closeModal() {
      vm.cleanForm();
      $mdDialog.cancel();
    }

    function cleanForm() {
      vm.task = new TaskService();
    }

    function setDone(task) {
      vm.resource = task;
      vm.save();
      //console.log(vm);
    }

  }

})();
