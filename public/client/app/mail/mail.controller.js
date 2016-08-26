(function() {

  'use strict';

  angular
    .module('app')
    .controller('MailController', MailController);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MailController($controller, MailService, UserService, PrDialog, $q, $timeout) {
    /*var vm = this;
    var cachedQuery;
    var pendingSearch;
    var cancelSearch = angular.noop;
    var allUsers;
    var lastSearch;

    vm.userDialog = userDialog;
    vm.addMail = addMail;
    vm.querySearch = querySearch;
    vm.remoteLoadUser = remoteLoadUser;
    vm.loadUser = loadUser;

    activate();

    function activate() {
      vm.mail = new MailService();
      vm.allUsers = remoteLoadUser();
    }

    function userDialog() {
      var config = {
        locals: {
          userDialogInput: {
            transferUserFn: vm.addMail
          }
        },
        controller: 'UsersDialogController',
        controllerAs: 'ctrl',
        templateUrl: '/users/users-dialog.html',
        hasBackdrop: true
      };

      PrDialog.custom(config);

    }

    function addMail(user) {
      console.log(user);
      vm.mail.users.push(user);
    }

    function querySearch(criteria) {
      cachedQuery = cachedQuery || criteria;
      return cachedQuery ? allUsers.filter(createFilterFor(cachedQuery)) : [];
    }

    function loadUser(criteria) {
      cachedQuery = criteria;
      if (!pendingSearch || !debounceSearch())  {
        cancelSearch();
        return pendingSearch = $q(function(resolve, reject) {
          // Simulate async search... (after debouncing)
          cancelSearch = reject;
          resolve(vm.querySearch());
          refreshDebounce();
        });
      }
      return pendingSearch;
    }

    function refreshDebounce() {
      lastSearch = 0;
      pendingSearch = null;
      cancelSearch = angular.noop;
    }

    function debounceSearch() {
      var now = new Date().getMilliseconds();

      lastSearch = lastSearch || now;
      return ((now - lastSearch) < 300);
    }

    /**
     * Create filter function for a query string

    function createFilterFor(query) {
      var lowercaseQuery = angular.lowercase(query);

      return function filterFn(contact) {
        return (contact._lowername.indexOf(lowercaseQuery) !== -1);
      };

    }

    function remoteLoadUser() {
      UserService.query().then(function(response) {
        allUsers = response;
        return allUsers.map(function (c) {
          var contact = {
            name: c.name,
            email: c.email
          };

          contact._lowername = contact.name.toLowerCase();
          return contact;
        })
      });
    }*/

    var vm = this;

    var pendingSearch, cancelSearch = angular.noop;
    var cachedQuery, lastSearch;

    vm.allContacts = loadContacts();
    vm.contacts = [vm.allContacts[0]];
    vm.asyncContacts = [];
    vm.filterSelected = true;

    vm.querySearch = querySearch;
    vm.delayedQuerySearch = delayedQuerySearch;

    /**
     * Search for contacts; use a random delay to simulate a remote call
     */
    function querySearch (criteria) {
      cachedQuery = cachedQuery || criteria;
      return cachedQuery ? vm.allContacts.filter(createFilterFor(cachedQuery)) : [];
    }

    /**
     * Async search for contacts
     * Also debounce the queries; since the md-contact-chips does not support this
     */
    function delayedQuerySearch(criteria) {
      cachedQuery = criteria;
      if (!pendingSearch || !debounceSearch())  {
        cancelSearch();

        return pendingSearch = $q(function(resolve, reject) {
          // Simulate async search... (after debouncing)
          cancelSearch = reject;
          $timeout(function() {

            resolve(vm.querySearch());

            refreshDebounce();
          }, Math.random() * 500, true)
        });
      }

      return pendingSearch;
    }

    function refreshDebounce() {
      lastSearch = 0;
      pendingSearch = null;
      cancelSearch = angular.noop;
    }

    /**
     * Debounce if querying faster than 300ms
     */
    function debounceSearch() {
      var now = new Date().getMilliseconds();

      lastSearch = lastSearch || now;

      return ((now - lastSearch) < 300);
    }

    /**
     * Create filter function for a query string
     */
    function createFilterFor(query) {
      var lowercaseQuery = angular.lowercase(query);

      return function filterFn(contact) {
        return (contact._lowername.indexOf(lowercaseQuery) !== -1);;
      };

    }

    function loadContacts() {
      var contacts = [
        'Marina Augustine',
        'Oddr Sarno',
        'Nick Giannopoulos',
        'Narayana Garner',
        'Anita Gros',
        'Megan Smith',
        'Tsvetko Metzger',
        'Hector Simek',
        'Some-guy withalongalastaname'
      ];

      return contacts.map(function (c, index) {
        var cParts = c.split(' ');

        var contact = {
          name: c,
          email: cParts[0][0].toLowerCase() + '.' + cParts[1].toLowerCase() + '@example.com',
          image: 'http://lorempixel.com/50/50/people?' + index
        };

        contact._lowername = contact.name.toLowerCase();
        return contact;
      });
    }

  }

})();
