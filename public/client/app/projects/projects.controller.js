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
        locals: { projectId: projectId },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        templateUrl: '/tasks/task-dialog.html',
        onRemoving: function() {
          vm.afterSave();
        }
      };

      PrDialog.custom(options);

    }

    function afterSave() {
      vm.cleanForm();
      vm.search(vm.paginator.currentPage);
    }

    function removeProject(resource) {
      var config = {
        title: 'Confirmar remoção',
        description: 'Deseja remover permanentemente o projeto '+ resource.name +'?',
        yesAction: function() {
          vm.remove(resource);
        }
      }

      PrDialog.confirm(config);
    }

  }

})();
