(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProjectsController', ProjectsController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function ProjectsController(PrToast, PrPagination, ProjectService) {
    var vm = this;

    vm.search     = search;
    vm.edit       = edit;
    vm.save       = save;
    vm.remove     = remove;
    vm.goTo       = goTo;
    vm.cleanForm  = cleanForm;

    activate();

    function activate() {
      vm.viewForm = false;
      vm.project = new ProjectService();

      vm.paginator = PrPagination.getInstance(search, 10);
      vm.search();
    }

    function search(page) {
      vm.paginator.currentPage = (angular.isDefined(page)) ? page : 1;

      ProjectService.paginate({
        page: vm.paginator.currentPage, perPage: vm.paginator.perPage
      }).then(function (response) {
        vm.paginator.calcNumberOfPages(response.total);
        vm.projects = response.items;
      }, function () {
        PrToast.error('Não foi possível realizar a busca de usuários');
      });
    }

    function cleanForm() {
      vm.project = new ProjectService();
    }

    function edit(project) {
      vm.cleanForm();
      vm.goTo('form');

      vm.project = angular.copy(project);

    }

    function save() {

      vm.project.$save().then(function (project) {
        vm.project = project;

        vm.cleanForm();
        vm.search(vm.paginator.currentPage);
        vm.goTo('list');
      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível salvar o projeto');
      });
    }

    function remove(project) {
      project.$remove().then(function () {
        vm.search();
        PrToast.info('Projeto removido');
      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível remover o projeto');
      });
    }

    function goTo(viewName) {
      vm.viewForm = (viewName === 'form');
    }
  }

})();
