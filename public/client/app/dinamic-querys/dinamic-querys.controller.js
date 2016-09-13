(function() {

  'use strict';

  angular
    .module('app')
    .controller('DinamicQuerysController', DinamicQuerysController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function DinamicQuerysController($controller, DinamicQueryService, lodash, PrToast) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.applyFilters = applyFilters;
    vm.loadAttributes = loadAttributes;
    vm.loadOperators = loadOperators;
    vm.addFilter = addFilter;
    vm.afterSearch = afterSearch;
    vm.editFilter = editFilter;
    vm.loadModels = loadModels;
    vm.removeFilter = removeFilter;
    vm.clearAll = clearAll;

    $controller('CRUDController', { vm: vm, modelService: DinamicQueryService, options: {
      searchOnInit: false
    } });

    function onActivate() {
      vm.keys = [];
      vm.addedFilters = [];
      vm.queryFilters = {};
      vm.index = -1;

      vm.loadModels();
    }

    function applyFilters(defaultQueryFilters) {
      if (vm.addedFilters.length > 0) {
        var addedFilters = angular.copy(vm.addedFilters);

        var where = { model: vm.addedFilters[0].model.name };

        for (var index = 0; index < addedFilters.length; index++) {
          var filter = addedFilters[index];

          filter.model = null;
          filter.attribute = filter.attribute.name;
          filter.operator = filter.operator.value;
        }

        where.filters = angular.toJson(addedFilters);
      } else {
        var where = { model: vm.models[0].name };
      }

      return angular.extend(defaultQueryFilters, where);
    }

    function loadModels() {
      //Pega todos os models do server e monta uma lista pro ComboBox
      DinamicQueryService.getModels().then(function(data) {
        vm.models = data;
        vm.queryFilters.model = vm.models[0];
        vm.loadAttributes();
      });
    }

    function loadAttributes() {
      vm.attributes = vm.queryFilters.model.attributes;
      vm.queryFilters.attribute = vm.attributes[0];

      vm.loadOperators();
    }

    function loadOperators() {
      var operators = [
        { value: '=', label: 'Igual' },
        { value: '<>', label: 'Diferente' },
        { value: '>', label: 'Maior' },
        { value: '>=', label: 'Maior ou Igual' },
        { value: '<', label: 'Menor' },
        { value: '<=', label: 'Menor ou Igual' }
      ]

      if (vm.queryFilters.attribute.type.indexOf('varying') !== -1) {
        operators.push({ value: 'like', label: 'Contém' });
      }

      vm.operators = operators;
      vm.queryFilters.operator = vm.operators[0];
    }

    function addFilter() {
      if (angular.isUndefined(vm.queryFilters.value) || vm.queryFilters.value === '') {
        PrToast.error('O campo valor é obrigratório');
      } else {
        if (vm.index < 0) {
          vm.addedFilters.push(angular.copy(vm.queryFilters));
        } else {
          vm.addedFilters[vm.index] = angular.copy(vm.queryFilters);
          vm.index = -1;
        }
        vm.queryFilters = {};
        vm.loadModels();
      }
    }

    function editFilter($index, queryFilters) {
      vm.index = $index;
      vm.queryFilters = queryFilters;
    }

    function removeFilter($index) {
      vm.addedFilters.splice($index);
    }

    function afterSearch(response) {
      var keys = (response.items.length > 0) ? Object.keys(response.items[0]) : [];

      vm.keys = lodash.filter(keys, function(key) {
        return !lodash.startsWith(key, '$');
      })
    }

    function clearAll() {
      vm.onActivate();
    }

  }

})();
