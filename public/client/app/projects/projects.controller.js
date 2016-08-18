(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController(Global, $controller, ProjectService, $mdDialog, $mdMedia, TaskService, PrToast) {
    var vm = this;

    //Attributes Block
    vm.task = {};

    //Functions Block
    vm.onActivate       = onActivate;
    vm.closeModalTask   = closeModalTask;
    vm.addTasks         = addTasks;
    vm.cleanFormTask    = cleanFormTask;
    vm.saveTask         = saveTask;
    vm.afterSave        = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function onActivate() {
      vm.task = new TaskService();
    }

    function addTasks(ev, idProject) {
      var idProject = idProject;
      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: vm,
        controller: angular.noop,
        controllerAs: 'projectsCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-form.html',
        targetEvent: ev,
        clickOutsideToClose: true,
        fullscreen: useFullScreen
      });

    }

    function afterSave() {
      vm.search(vm.paginator.currentPage);
    }

    function closeModalTask() {
      vm.task = new TaskService();
      $mdDialog.cancel();
    }

    function cleanFormTask(task) {
      console.log(vm.task);
      vm.task = new TaskService();
      console.log(task);
    }

    function saveTask() {
      vm.task.$save().then(function (resource) {
        vm.task = resource;

        vm.cleanFormTask();
        vm.search(vm.paginator.currentPage);

        PrToast.success('Operação realizada com sucesso');

      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível salvar.');
      });
    }

  }

})();
