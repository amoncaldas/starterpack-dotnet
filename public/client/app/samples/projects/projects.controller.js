(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController($controller, ProjectsService, PrDialog, $translate) {
    var vm = this;

    //Attributes Block

    //Functions Block
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;
    vm.removeProject = removeProject;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectsService, options: { } });

    function viewTasks(projectId) {
      var config = {
        locals: {
          projectId: projectId
        },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        templateUrl: '/samples/tasks/tasks-dialog.html',
        hasBackdrop: true
      };

      PrDialog.custom(config).then(function() {
        vm.afterSave();
      });

    }

    function afterSave() {
      vm.cleanForm();
      vm.search(vm.paginator.currentPage);
    }

    function removeProject(resource) {
      var config = {
        title: $translate.instant('project.confirm.title'),
        description: $translate.instant('project.confirm.description', resource)
      }

      PrDialog.confirm(config).then(function() {
        vm.remove(resource);
      });
    }

  }

})();
