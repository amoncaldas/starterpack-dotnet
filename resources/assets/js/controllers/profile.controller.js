(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProfileController', ProfileController);

  ProfileController.$inject = ['UserService', 'Auth', 'Toast'];

  function ProfileController(UserService, Auth, Toast) {
    var vm = this;

    vm.update = update;

    activate();

    function activate() {
      vm.user = angular.copy(Auth.currentUser);
    }

    function update() {
      UserService.updateProfile(vm.user).then(function (response) {
        Auth.updateCurrentUser(response.data);
      }, function (error) {
        Toast.errorValidation(error.data, "Não foi possível atualizar seu profile");
      });
    }
  }

})();
