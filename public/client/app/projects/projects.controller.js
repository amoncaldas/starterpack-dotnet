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

    //Functions Block
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function viewTasks(event, projectId) {
      vm.currentProjectId = projectId;

      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: { projectCtrl: vm },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-dialog.html',
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
