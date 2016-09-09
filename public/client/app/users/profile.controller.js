(function() {

  'use strict';

  angular
    .module('app')
    .controller('ProfileController', ProfileController);

  /** @ngInject */
  function ProfileController(UsersService, Auth, PrToast, $translate) {
    var vm = this;

    vm.update = update;

    activate();

    function activate() {
      vm.user = angular.copy(Auth.currentUser);
    }

    function update() {
      UsersService.updateProfile(vm.user).then(function (response) {
        Auth.updateCurrentUser(response);
        PrToast.success($translate.instant('controllers.crud.saveSuccess'));
      }, function (error) {
        PrToast.errorValidation(error.data, $translate('user.profile.updateError'));
      });
    }
  }

})();
