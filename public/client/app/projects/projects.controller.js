(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController(Global, $controller, ProjectService, $mdDialog, $mdMedia) {
    var vm = this;

    //Attributes Block
    vm.task = {};

    //Functions Block
    vm.addTasks  = addTasks;
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function addTasks(event, projectId) {
      vm.currentProjectId = projectId;
      vm.searchOnInit = false;

      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: { projectCtrl: vm },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-dialog-form.html',
        targetEvent: event,
        clickOutsideToClose: true,
        fullscreen: useFullScreen
      });

    }

    function viewTasks(event, projectId) {
      vm.currentProjectId = projectId;
      vm.searchOnInit = true;

      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: { projectCtrl: vm },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-dialog-list.html',
        targetEvent: event,
        clickOutsideToClose: true,
        fullscreen: useFullScreen
      });

    }

    function afterSave() {
      vm.search(vm.paginator.currentPage);
    }

  }

})();
