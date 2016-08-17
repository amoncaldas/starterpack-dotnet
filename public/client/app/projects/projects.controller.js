(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController($controller, ProjectService) {
    var vm = this;

    //Functions Block
    vm.openModalTask = openModalTask;

    // instantiate base controller
    $controller('CRUDController', { vm: vm, modelService: ProjectService });

    function openModalTask() {
      console.log('TESTE');
      /*var useFullScreen = ($mdMedia('sm') || $mdMedia('xs'));

      $mdDialog.show({
        controller: ProjectsController,
        templateUrl: 'client/projects/task-form.html',
        parent: angular.element(angular.document.body),
        targetEvent: ev,
        clickOutsideToClose: true,
        fullscreen: useFullScreen
      });*/
    };

    vm.openModal = function() {
      console.log('Teste');
    }
  }

})();
