(function() {

  'use strict';

  angular
    .module('app')
    .controller('DinamicQuerysController', DinamicQuerysController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function DinamicQuerysController($controller, DinamicQueryService, lodash, PrToast) {
    var vm = this;

    //actions
    vm.onActivate = onActivate;
    vm.applyFilters = applyFilters;
    vm.loadAttributes = loadAttributes;
    vm.loadOperators = loadOperators;
    vm.addFilter = addFilter;
    vm.afterSearch = afterSearch;
    vm.editFilter = editFilter;
    vm.loadModels = loadModels;
    vm.removeFilter = removeFilter;
    vm.clear = clear;
    vm.restart = restart;

    //herda o comportamento base do CRUD
    $controller('CRUDController', { vm: vm, modelService: DinamicQueryService, options: {
      searchOnInit: false
    } });

    function onActivate() {      
      vm.restart();      
    }

    /**
     * Prepara e aplica os filtro que vão ser enviados para o serviço
     * 
     * @param {any} defaultQueryFilters
     * @returns
     */
    function applyFilters(defaultQueryFilters) {
      var where = {};

      /**
       * o serviço espera um objeto com:
       *  o nome de um model 
       *  uma lista de filtros
       */
      if (vm.addedFilters.length > 0) {
        var addedFilters = angular.copy(vm.addedFilters);

        where.model = vm.addedFilters[0].model.name;

        for (var index = 0; index < addedFilters.length; index++) {
          var filter = addedFilters[index];

          filter.model = null;
          filter.attribute = filter.attribute.name;
          filter.operator = filter.operator.value;
        }

        where.filters = angular.toJson(addedFilters);
      } else {
        where.model = vm.models[0].name;
      }

      return angular.extend(defaultQueryFilters, where);
    }

    /**
     * Carrega todos os models criados no servidor com seus atributos 
     */
    function loadModels() {
      //Pega todos os models do server e monta uma lista pro ComboBox
      DinamicQueryService.getModels().then(function(data) {
        vm.models = data;
        vm.queryFilters.model = vm.models[0];
        vm.loadAttributes();
      });
    }
    
    /**
     * Carrega os attributos do model escolhido
     */
    function loadAttributes() {
      vm.attributes = vm.queryFilters.model.attributes;
      vm.queryFilters.attribute = vm.attributes[0];

      vm.loadOperators();
    }

    /**
     * Carrega os operadores especificos para o tipo do atributo
     */
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

    /**
     * Adiciona/edita um filtro  
     * 
     * @param {any} form elemento html do formulário para validações 
     */
    function addFilter(form) {
      if (angular.isUndefined(vm.queryFilters.value) || vm.queryFilters.value === '') {
        PrToast.error('O campo valor é obrigratório');
      } else {
        if (vm.index < 0) {
          vm.addedFilters.push(angular.copy(vm.queryFilters));
        } else {
          vm.addedFilters[vm.index] = angular.copy(vm.queryFilters);
          vm.index = -1;
        }
                
        //reinicia o formulário e as validações existentes
        vm.queryFilters = {};
        form.$setPristine();
        form.$setUntouched();             
      }
    }

    /**
     * Gatilho acionado depois da pesquisa responsável por identificar os atributos 
     * contidos nos elementos resultantes da busca
     * 
     * @param {any} data dados referente ao retorno da requisição
     */
    function afterSearch(data) {
      var keys = (data.items.length > 0) ? Object.keys(data.items[0]) : [];

      //retira todos os atributos que começam com $. 
      //Esses atributos são adicionados pelo serviço e não deve aparecer na listagem
      vm.keys = lodash.filter(keys, function(key) {
        return !lodash.startsWith(key, '$');
      })
    }    

    /**
     * Coloaca no formulário o filtro escolhido para edição
     * @param {any} $index indice no array do filtro escolhido
     */
    function editFilter($index) {
      vm.index = $index;     
      vm.queryFilters = vm.addedFilters[$index];
    }

    /**
     * Remove o filtro escolhido
     * 
     * @param {any} $index indice no array do filtro escolhido
     */
    function removeFilter($index) {
      vm.addedFilters.splice($index);
    }

    /**
     * Limpa o formulário corrente
     */
    function clear() {
      //guarda o indice do registro que está sendo editado
      vm.index = -1;
      //vinculado aos campos do formulário
      vm.queryFilters = {};
    }

    /**
     * Reinicia a construção da query limpando tudo
     * 
     */
    function restart() {
      //guarda atributos do resultado da busca corrente
      vm.keys = [];

      //guarda os filtros adicionados
      vm.addedFilters = [];
      vm.clear();
      vm.loadModels();
    }

  }

})();
