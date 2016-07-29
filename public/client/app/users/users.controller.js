(function() {

  'use strict';

  angular
    .module('app')
    .controller('UsersController', UsersController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function UsersController(lodash, PrToast, PrPagination, UserService, RoleService, Auth) {
    var vm = this;

    vm.search = search;
    vm.edit = edit;
    vm.save = save;
    vm.remove = remove;
    vm.goTo = goTo;
    vm.cleanForm = cleanForm;

    activate();

    function activate() {
      vm.viewForm = false;
      vm.user = new UserService();

      vm.roles = RoleService.query().$promise.then(function (response) {
        vm.roles = response;
      });

      vm.paginator = PrPagination.getInstance(search, 2);
      vm.search(1);
    }

    function search(page) {
      vm.paginator.currentPage = page;

      UserService.paginate({ page: page, perPage: vm.paginator.perPage }).$promise.then(function (response) {
        vm.paginator.calcNumberOfPages(response.total);
        vm.users = response.items;
      }, function () {
        PrToast.error('Não foi possível realizar a busca de usuários');
      });
    }

    function cleanForm() {
      vm.user = new UserService();

      vm.roles.forEach(function(role) {
        role.selected = false;
      });
    }

    function edit(user) {
      vm.cleanForm();
      vm.goTo('form');

      vm.user = angular.copy(user);

      vm.roles.forEach(function(role) {
        vm.user.roles.forEach(function(roleUser) {
          if (role.id === roleUser.id) {
            role.selected = true;
          }
        });
      });
    }

    function save() {
      var promise;

      vm.user.roles = lodash.map(lodash.filter(angular.copy(vm.roles), { selected: true }), 'id');
      promise = (vm.user.id) ? vm.user.$update() : vm.user.$save();

      promise.then(function (user) {
        vm.user = user;

        if (vm.user.id === Auth.currentUser.id) {
          Auth.updateCurrentUser(user.plain());
        }

        vm.cleanForm();
        vm.search();
      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível salvar o usuário');
      });
    }

    function remove(user) {
      var promise;

      promise = (user.id) ? user.$remove() : PrToast.error('Nenhum usuário selecionado para deletar');
      promise.then(function () {
        if (user.id === Auth.currentUser.id) {
          Auth.updateCurrentUser(user.plain());
        }
        vm.search();
        PrToast.info('Usuário removido');
      }, function (error) {
        PrToast.errorValidation(error.data, 'Não foi possível remover o usuário');
      });
    }

    function goTo(viewName) {
      vm.viewForm = (viewName === 'form');
    }
  }

})();
