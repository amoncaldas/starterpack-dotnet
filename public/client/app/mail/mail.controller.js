(function() {

  'use strict';

  angular
    .module('app')
    .controller('MailController', MailController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MailController($controller, UserService, MailService) {
    var vm = this;

    vm.mail = new MailService();
    vm.user = new UserService();
    vm.users = [];

    //Functions block
    vm.send = send;
    vm.searchUsers = searchUsers;

    // instantiate base controller
    $controller('CRUDController', {
      vm: vm.user,
      modelService:
      UserService,
      options: {
        searchOnInit: false
      }
    });

    send();
    function send() {
      //angular.extend(vm.user.defaultQueryFilters, { name: 'francis' });
      vm.user.search(vm.user.paginator.currentPage);
      console.log(vm.user);
    }

    function searchUsers() {
      vm.users = UserService.search();
    }

  }

})();
