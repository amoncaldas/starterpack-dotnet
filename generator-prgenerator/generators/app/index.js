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

    var done = me.async();

    me.log(utils.logoProdeb());

    //Montando as perguntas que serão exibidas no prompt
    var questions = prPrompt.mountQuestions();

    var validNameResource = function() {
      var resources = me.props.resourceName.split(":");
      var regex = new RegExp('^[a-zA-Z]+:[a-zA-Z]+$');
      var error = false;

      if (resources.length > 1 && !regex.test(me.props.resourceName)) {
        me.log(chalk.red('Nome do recurso é inválido, tente novamente.'));
        error = true;
      }

      regex = new RegExp('^[a-zA-Z]+$');
      if (resources.length == 1 && !regex.test(me.props.resourceName)) {
        me.log(chalk.red('Nome do recurso é inválido, tente novamente.'));
        error = true;
      }

      return error;
    }

    var verify = function(pathNotExists, htmlController) {

      var asks = [];
      if (pathNotExists === true && !htmlController) asks = [questions.structure];
      if (pathNotExists === false && htmlController) asks = [questions.htmlConfirm];
      if (pathNotExists !== true && !htmlController) asks = [questions.name];

      var prompt = me.prompt(asks).then(function (answers) {
        if (answers.structure === 'exit') {
          console.log(chalk.cyan.bold('\n\n#######      Obrigado por ter usado nosso gerador!      #######\n'));
          process.exit();
        }
        if (answers.structure) me.props.structure = answers.structure;
        if (answers.htmlController !== undefined) me.props.htmlController = answers.htmlController;
        if (answers.resourceName) me.props.resourceName = answers.resourceName;

        if (me.props.structure === 'controller' && me.props.htmlController === undefined) {
          verify(false, true);
        } else if (me.props.resourceName === '') {
          verify(false, false);
        } else if (me.props.resourceName !== '' && validNameResource()) {
          verify(false, false);
        } else {

        //Verifica se o diretório ou arquivo já existe.
        file.verifyDirAndFile(me.prefixPath, me.props).then(response => {
          if (response === true) {
            me.path = file.createDirectory(me.prefixPath, me.props);
            done();
          } else {
            me.log(response);
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

    file.copyFiles(this);

  },

  end: function() {
    this.log(chalk.green('\n\n#######      Operação realizada com sucesso!      #######\n\n'));
  }

});

module.exports = PrGenerator;
