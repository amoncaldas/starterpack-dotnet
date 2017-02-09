(function() {
  'use strict';

  angular
    .module('app')
    .factory('Auth', Auth);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function Auth($http, jwtHelper, $q, Global, UsersService) { // NOSONAR
    var auth = {
      login: login,
      logout: logout,
      updateCurrentUser: updateCurrentUser,
      retrieveUserFromLocalStorage: retrieveUserFromLocalStorage,
      authenticated: authenticated,
      sendEmailResetPassword: sendEmailResetPassword,
      currentUser: null,
      getToken: getToken,
      setToken: setToken,
      clearToken: clearToken
    };

    var data = {
      token: null
    }

    function clearToken() {
      localStorage.removeItem(Global.tokenKey);
      data.token = null;
    }

    function setToken(token) {
      localStorage.setItem(Global.tokenKey, token);
      data.token = token;
    }

    function getToken() {
      var token = data.token;

      if (!token) {
        token = localStorage.getItem(Global.tokenKey);
      }

      return token;
    }

    /**
     * Verifica se o usuário está autenticado
     *
     * @returns {boolean}
     */
    function authenticated() {
      var token = auth.getToken();

      return (token && !jwtHelper.isTokenExpired(token));
    }

    /**
     * Recupera o usuário do localStorage
     */
    function retrieveUserFromLocalStorage() {
      var user = localStorage.getItem('user');

      if (user) {
        auth.currentUser = angular.merge(new UsersService(), angular.fromJson(user));
      }
    }

    /**
     * Guarda o usuário no localStorage para caso o usuário feche e abra o navegador
     * dentro do tempo de sessão seja possível recuperar o token autenticado.
     *
     * Mantém a variável auth.currentUser para facilitar o acesso ao usuário logado em toda a aplicação
     *
     *
     * @param {any} user Usuário a ser atualizado. Caso seja passado null limpa
     * todas as informações do usuário corrente.
     */
    function updateCurrentUser(user) {
      var deferred = $q.defer();

      if (user) {
        user = angular.merge(new UsersService(), user);

        var jsonUser = angular.toJson(user);

        localStorage.setItem('user', jsonUser);
        auth.currentUser = user;

        deferred.resolve(user);
      } else {
        localStorage.removeItem('user');
        auth.currentUser = null;
        auth.clearToken();

        deferred.reject();
      }

      return deferred.promise;
    }

    /**
     * Realiza o login do usuário
     *
     * @param {any} credentials Email e Senha do usuário
     * @returns {promise} Uma promise com o resultado do chamada no backend
     */
    function login(credentials) {
      var deferred = $q.defer();

      $http.post(Global.apiPath + '/authenticate', credentials)
        .then(function(response) {
          auth.setToken(response.data.token);

          return $http.get(Global.apiPath + '/authenticate/user');
        })
        .then(function(response) {
          auth.updateCurrentUser(response.data.user);

          deferred.resolve();
        }, function(error) {

          auth.clearToken();
          deferred.reject(error);
        });

      return deferred.promise;
    }

    /**
     * Desloga os usuários. Como não ten nenhuma informação na sessão do servidor
     * e um token uma vez gerado não pode, por padrão, ser invalidado antes do seu tempo de expiração,
     * somente apagamos os dados do usuário e o token do navegador para efetivar o logout.
     *
     * @returns {promise} Uma promise com o resultado da operação
     */
    function logout() {
      var deferred = $q.defer();

      auth.updateCurrentUser(null);
      auth.clearToken();

      deferred.resolve();


      return deferred.promise;
    }

    /**
     * Envia um email para recuperação de senha
     * @param {Object} resetData - Objeto contendo o email
     * @return {Promise} - Retorna uma promise para ser resolvida
     */
    function sendEmailResetPassword(resetData) {
      var deferred = $q.defer();

      $http.post(Global.apiPath + '/password/email', resetData)
        .then(function(response) {
          deferred.resolve(response);
        }, function(error) {
          deferred.reject(error);
        });

      return deferred.promise;

    }

    return auth;
  }

}());
