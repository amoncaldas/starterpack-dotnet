(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController($controller, ProjectService, PrDialog) {
    var vm = this;

    //Attributes Block

    //Functions Block
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;
    vm.removeProject = removeProject;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function viewTasks(projectId) {
      var options = {
        locals: {
          projectId: projectId
        },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        templateUrl: '/tasks/task-dialog.html'
      };

      PrDialog.custom(options).then(function() {

      }, function() {
        vm.afterSave();
      });

    }

    function afterSave() {
      vm.cleanForm();
      vm.search(vm.paginator.currentPage);
    }

    function removeProject(resource) {
      var config = {
        title: 'Confirmar remoção',
        description: 'Deseja remover permanentemente o projeto '+ resource.name +'?'
      }

      PrDialog.confirm(config).then(function() {
        vm.remove(resource);
      });
    }

  }

})();
