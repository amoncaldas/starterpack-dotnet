(function() {

  'use strict';

  angular
    .module('app')
    .controller('UsersController', UsersController);

  UsersController.$inject = ['lodash', 'Toast', 'UserService', 'RoleService', 'Auth'];

  function UsersController(_, Toast, UserService, RoleService, Auth) {
    var vm = this;

    vm.search = search;
    vm.edit = edit;
    vm.save = save;
    vm.cleanForm = cleanForm;

    activate();

    function activate() {
      vm.user = {};

      vm.roles = RoleService.query().$promise.then(function (response) {
        vm.roles = response;
      });
      vm.search();
    }

    function search() {
      UserService.query().$promise.then(function (response) {
        vm.users = response;
      }, function (error) {
        Toast.error("Não foi possível realizar a busca de usuários");
      });
    }

    function cleanForm() {
      vm.user = {};

      vm.roles.forEach(function(role) {
        role.selected = false;
      });
    }

    function edit(user) {
      vm.cleanForm();
      vm.user = user;

      vm.roles.forEach(function(role) {
        vm.user.roles.forEach(function(roleUser) {
          if(role.id === roleUser.id) { role.selected = true; }
        });
      });
    }

    function save() {
      var promisse;

      vm.user.roles = _.pluck(_.filter(angular.copy(vm.roles), { 'selected': true }),'id');

      promisse = (vm.user.id) ?  UserService.update(vm.user).$promise : UserService.save(vm.user).$promise;

      promise.then(function (user) {
        vm.user = user;

        if(vm.user.id === Auth.currentUser.id) { Auth.updateCurrentUser(user.plain()); }

        vm.cleanForm();
        vm.search();
      }, function (error) {
        Toast.errorValidation(error.data, "Não foi possível salvar o usuário");
      });
    }
  }

})();
