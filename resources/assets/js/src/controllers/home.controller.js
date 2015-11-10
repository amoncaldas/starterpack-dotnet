(function() {

  'use strict';

  angular
    .module('app')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$http', '$auth', '$rootScope', '$state'];

  function HomeController($http, $auth, $rootScope, $state) {
    var vm = this;

    vm.users = null;
    vm.error = null;

    vm.getUsers = getUsers;

    function getUsers(flag) {

      // This request will hit the index method in the AuthenticateController
      // on the Laravel side and will return the list of users
      if(flag) {
        $http.get('api/authenticate').success(function (users) {
          vm.users = users;
        }).error(function (error) {
          vm.error = error;
        });
      } else {
        $http.get('api/authenticate/teste').success(function (users) {
          vm.users = users;
        }).error(function (error) {
          vm.error = error;
        });
      }
    }
  }

})();
