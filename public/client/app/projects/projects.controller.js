(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController(Global, $controller, ProjectService, $mdDialog, $mdMedia, TaskService) {
    var vm = this;

    //Attributes Block
    vm.task = {};

    //Functions Block
    vm.onActivate       = onActivate;
    vm.addTasks         = addTasks;
    vm.afterSave        = afterSave;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService, options: { redirectAfterSave: false } });

    function onActivate() {
      vm.task = new TaskService();
    }

    function addTasks(event, projectId) {
      vm.currentProjectId = projectId;
      var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        locals: { projectCtrl: vm },
        controller: function(projectCtrl) {
          var vm = this;

          vm.closeModal = closeModal;
          vm.beforeSave = beforeSave;

          $controller('CRUDController', { vm: vm, modelService: TaskService, options: {
            redirectAfterSave: false,
            searchOnInit: false
          } });

          function beforeSave() {
            vm.resource.project = { id: projectCtrl.currentProjectId };
          }

          function closeModal() {
            vm.task = new TaskService();
            $mdDialog.cancel();
          }
        },
        controllerAs: 'tasksCtrl',
        bindToController: true,
        templateUrl: Global.clientPath + '/tasks/task-form.html',
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
