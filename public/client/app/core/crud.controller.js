(function() {

  'use strict';

  angular
    .module('app')
    .controller('CRUDController', CRUDController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function CRUDController(vm, modelService, options, PrToast, PrPagination) {

    //Functions Block
    vm.search = search;
    vm.edit = edit;
    vm.save = save;
    vm.remove = remove;
    vm.goTo = goTo;
    vm.cleanForm = cleanForm;

    activate();

    function activate() {
      vm.defaultOptions = {
        redirectAfterSave: true,
        searchOnInit: true
      }

      angular.merge(vm.defaultOptions, options);

      vm.viewForm = false;
      vm.resource = new modelService();

      if (angular.isFunction(vm.onActivate)) vm.onActivate.call();

      vm.paginator = PrPagination.getInstance(search, vm.defaultOptions.perPage);

      if (vm.defaultOptions.searchOnInit) vm.search();
    }

    function search(page) {
      vm.paginator.currentPage = (angular.isDefined(page)) ? page : 1;
      vm.defaultQueryFilters = { page: vm.paginator.currentPage, perPage: vm.paginator.perPage };

      if (angular.isFunction(vm.beforeSearch) && vm.beforeSearch.call(page) === false) return false;

      modelService.paginate(vm.defaultQueryFilters).then(function (response) {
        vm.paginator.calcNumberOfPages(response.total);
        vm.resources = response.items;

        if (angular.isFunction(vm.afterSearch)) vm.afterSearch.call(response);
      }, function () {
        PrToast.error('Não foi possível realizar a busca.');
      });
    }

    function cleanForm() {
      vm.resource = new modelService();

      if (angular.isFunction(vm.afterClean)) vm.afterClean.call();
    }

    function edit(resource) {
      vm.goTo('form');

      vm.resource = angular.copy(resource);

      if (angular.isFunction(vm.afterEdit)) vm.afterEdit.call();
    }

    function save() {
      if (angular.isFunction(vm.beforeSave) && vm.beforeSave.call() === false) return false;

      vm.resource.$save().then(function (resource) {
        vm.resource = resource;

        if (angular.isFunction(vm.afterSave)) vm.afterSave.call(resource);

        if (vm.defaultOptions.redirectAfterSave) {
          vm.cleanForm();
          vm.search(vm.paginator.currentPage);
          vm.goTo('list');
        }

        PrToast.success('Operação realizada com sucesso');

      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível salvar.');
      });
    }

    function remove(resource) {
      if (angular.isFunction(vm.beforeRemove) && vm.beforeRemove.call(resource) === false) return false;

      resource.$destroy().then(function () {
        vm.search();
        PrToast.info('Remoção realizada com sucesso.');
      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível remover.');
      });
    }

    function goTo(viewName) {
      vm.viewForm = false;

      if (viewName === 'form') {
        vm.cleanForm();
        vm.viewForm = true;
      }
    }
  }

})();
