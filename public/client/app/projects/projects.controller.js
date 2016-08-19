(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController(Global, $controller, ProjectService, $mdDialog, $mdMedia, PrDialog) {
    var vm = this;

    //Attributes Block

    //Functions Block
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function viewTasks(projectId) {

      PrDialog.show('alert');

      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: { projectId: projectId },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-dialog.html',
        clickOutsideToClose: true,
        fullscreen: useFullScreen
      });

    }

    function afterSave() {
      vm.search(vm.paginator.currentPage);
    }

  }

})();
