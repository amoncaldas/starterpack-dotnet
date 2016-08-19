(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  //function ProjectsController(Global, $controller, ProjectService, $mdDialog, $mdMedia, PrDialog) {
  function ProjectsController($controller, ProjectService, PrDialog) {
    var vm = this;

    //Attributes Block

    //Functions Block
    vm.viewTasks = viewTasks;
    vm.afterSave = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    //function viewTasks() {
    function viewTasks(projectId) {

      var options = {
        locals: { projectId: projectId },
        controller: 'TasksDialogController',
        controllerAs: 'tasksCtrl',
        templateUrl: '/tasks/task-dialog.html'
      };

      PrDialog.show('custom', options)
        .then(function(response) {
          console.log(response);
          vm.search(vm.paginator.currentPage);
        });

    }

    function afterSave() {
      vm.search(vm.paginator.currentPage);
    }

  }

})();
