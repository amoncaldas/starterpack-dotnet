// spec.js

var helper = require('../helper');
var data = require('../data');
var localStorage = require("../localStorage")
var LoginPage = require('../login/login.page');
var UserPage = require('../users/users.page');

describe('Users Page', function() {
  var loginPage = new LoginPage();
  var usersPage = new UserPage();

  describe('normal user', function() {
    beforeAll(function() {
      loginPage.logout();
      loginPage.visit();
      loginPage.login({
        email: 'normal-base@prodeb.com'
      });
    });

    beforeEach(function() {
      usersPage.visit().then(function() {
        browser.sleep(1000);
      });
    })

    it('should not have access to users page', function() {
      expect(element(by.id('page-breadcrumb')).getText()).toEqual('Acesso Negado');
    });
  });

  describe('admin user', function() {

    var updateEmail = 'emailatualizado@prodeb.ba.gov.br';
    var totalUsers = 5;
    var totalUsersWithSearchUsuarioCriteria = 2;

    beforeAll(function() {
      loginPage.logout();
      loginPage.visit();
      loginPage.login();
    });

    beforeEach(function() {
      usersPage.visit().then(function() {
        browser.sleep(1000);
      });
    })

    it('should visit users page', function() {
      expect(element(by.id('page-breadcrumb')).getText()).toEqual('Administração - Usuário');
    });

    it('should load users list', function() {
      expect(usersPage.resourcesList.count()).toBe(totalUsers);
    });

    it('should search users list', function() {
      usersPage.search('usuario');

      expect(usersPage.resourcesList.count()).toBe(totalUsersWithSearchUsuarioCriteria);

      usersPage.search(data.validAdminUser.email);

      expect(usersPage.resourcesList.count()).toBe(1);

      usersPage.search('strangenamewithnosense');

      expect(usersPage.resourcesList.count()).toBe(0);
    });

    it('should save new user with valid data', function() {
      usersPage.save();
      helper.expectToastToEqual('Registro salvo com sucesso.');
    });

    it('should update a user', function() {
      usersPage.update({
        name: 'Nome Atualizado',
        email: updateEmail
      });

      browser.sleep(1000);
      helper.expectToastToEqual('Registro salvo com sucesso.');

      //get user list and try to find user's mail saved
      var userFound = usersPage.findUserByEmailInList(updateEmail);

      expect(userFound).toBeDefined();
    });

    it('should remove a user', function() {
      usersPage.resourcesList.count().then(function(count) {
        var totalBeforeUpdate = count;

        usersPage.remove(updateEmail);

        helper.expectToastToEqual('Remoção realizada com sucesso.');
        expect(usersPage.resourcesList.count()).toBe(totalBeforeUpdate - 1);
      });
    });

  });

});
