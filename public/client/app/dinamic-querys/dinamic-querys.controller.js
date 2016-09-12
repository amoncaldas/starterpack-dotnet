(function() {

  'use strict';

  angular
    .module('app')
    .controller('DinamicQueryController', DinamicQueryController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function DinamicQueryController($controller, DinamicQueryService, lodash) {
    var vm = this;

    vm.onActivate = onActivate;
    vm.applyFilters = applyFilters;
    vm.loadAttributes = loadAttributes;
    vm.loadOperators = loadOperators;
    vm.addFilter = addFilter;  
    vm.afterSearch = afterSearch;

    $controller('CRUDController', { vm: vm, modelService: DinamicQueryService, options: {
      searchOnInit: false
    } });

    function onActivate() {
      vm.keys = [];
      vm.addedFilters = [];
      vm.queryFilters = {};

      //Pega todos os models do server e monta uma lista pro ComboBox
      DinamicQueryService.getModels().then(function(data) {
        vm.models = data;
        vm.queryFilters.model = vm.models[0];
        vm.loadAttributes();
      });          
    }

    function applyFilters(defaultQueryFilters) {
      if( vm.addedFilters.length > 0 ) {
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

      if( vm.queryFilters.attribute.type.indexOf('varying') !== -1)
        operators.push( { value: 'like', label: 'Contém' } );

      vm.operators = operators;
      vm.queryFilters.operator = vm.operators[0];
    }

    function addFilter() {
      vm.addedFilters.push(angular.copy(vm.queryFilters));
    }

    function afterSearch(response) {
      var keys = (response.items.length > 0) ? Object.keys(response.items[0]) : [];

      vm.keys = lodash.filter(keys, function(key) {
        return !lodash.startsWith(key, '$');
      })       
    }

  }

})();
