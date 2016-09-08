'use strict';
//Require dependencies
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var utils = require('./utils');
var file = require('./file');
var prPrompt = require('./prPrompt');
var Promise = require('promise');

var PrGenerator =  yeoman.Base.extend({

  prompting: function() {
    var me = this;
    me.props = {resourceName:'', structure:''};
    me.prefixPath = "public/client/app/";

    /**
     * Faz uma chamada para o método assíncrono para garantir que
     * a função não termine antes de concluir todo o trabalho
     */
    var done = me.async();

    /**
     * Imprimi o logo da prodeb no console
     */
    me.log(utils.logoProdeb());

    //Montando as perguntas que serão exibidas no prompt
    var questions = prPrompt.mountQuestions();

    /**
     * Método faz o tratamento do prompt(exibição, validação e confirmação)
     * @param {boolean, boolean} pathNotExists, htmlController
     * @return {Promise} - Retorna uma promise para o prompting
     */
    var verify = function(pathNotExists, htmlController) {

      var asks = [];

      /**
       * Preenche o array asks com as questões a partir das validações
       * validação de o caminho ou o arquivo ja existe e se a estrutura
       * escolhida foi de controller para exibir a confirmação da criação
       * dos arquivos html
       */
      if (pathNotExists === true && !htmlController)
        asks = [questions.structure];
      else if (pathNotExists === false)
        asks = (htmlController) ? [questions.htmlConfirm] : [questions.name];

      /**
       * Resolvendo a promise das questões exibidas no console
       * e realiza as validações
       */
      var prompt = me.prompt(asks).then(function (answers) {
        if (answers.structure === 'exit') {
          me.log(chalk.cyan.bold('\n\n#######      Obrigado por ter usado nosso gerador!      #######\n'));
          process.exit();
        }
        if (answers.structure) me.props.structure = answers.structure;
        if (answers.htmlController !== undefined) me.props.htmlController = answers.htmlController;
        if (answers.resourceName) me.props.resourceName = answers.resourceName;

        /**
         * Realiza a chamada da função verify() recursivamente, para que todas as questões
         * sejam respondidas e validadas
         */
        if (me.props.structure === 'controller' && me.props.htmlController === undefined) {
          verify(false, true);
        } else if (me.props.resourceName === '') {
          verify(false, false);
        } else if (me.props.resourceName !== '' && utils.validNameResource()) {
          verify(false, false);
        } else {

        //Verifica se o diretório ou arquivo já existe.
        file.verifyDirAndFile(me.prefixPath, me.props).then(response => {
          //Caso o diretório ou arquivo não exista é criado um novo diretório e pula para o método writing
          if (response === true) {
            me.path = file.createDirectory(me.prefixPath, me.props);
            done();
          } else {
            //Caso contrário o erro é impresso no console e a função verify é chamada
            me.log(response);

            if (typeof response === 'string')
              response = false;

            verify(response, false);
          }
        });

      }

      }.bind(me));

      return prompt;
    }

    return verify(true, false);

  },

  writing: function() {
    //Realiza a copia dos arquivo de template
    file.copyFiles(this);

  },

  end: function() {
    this.log(chalk.green('\n\n#######      Operação realizada com sucesso!      #######\n\n'));
  }

});

module.exports = PrGenerator;
