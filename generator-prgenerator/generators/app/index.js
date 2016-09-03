'use strict';
//Require dependencies
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var mkdirp = require('mkdirp');
var utils = require('./utils');

var PrGenerator =  yeoman.Base.extend({

  prompting: function() {
    var done = this.async();
    var prefixPath = "public/client/app/";

    this.log(utils.logoProdeb());

    //Montando as perguntas que serão exibidas no prompt
    var askList = {
      type: 'list',
      name: 'structure',
      message: chalk.yellow('Escolha qual estrutura a ser gerada?'),
      choices: [
        {name: chalk.yellow('Gerar estrutura completa.'), value: 'complete'},
        {name: chalk.yellow('Gerar estrutura html.'), value: 'html'},
        {name: chalk.yellow('Gerar estrutura do controllerJS.'), value: 'controller'},
        {name: chalk.yellow('Gerar estrutura do serviceJS.'), value: 'service'},
        {name: chalk.yellow('Gerar estrutura de routeJS.'), value: 'route'},
        {name: chalk.yellow('Sair.'), value: 'exit'}
      ]
    };
    var askName = {
      when: function(answer) {
        if (answer.structure === 'exit') {
          console.log(chalk.cyan.bold('\n\n#######      Obrigado por ter usado nosso gerador!      #######\n'));
          process.exit();
        }
        return true;
      },
      type: 'input',
      name: 'resourceName',
      message: chalk.yellow('Digite o nome do recurso?'),
      validate: function (value) {
        return utils.verifyInput(value);
      }
    };

    var verify = function(askList, askName, me, notExists) {

      var asks = notExists === true ? [askList, askName] : [askName];

      var prompt = me.prompt(asks).then(function (answers) {
        me.props = answers;

        if (me.props.structure === 'exit') process.exit();

        me.path = prefixPath + me.props.resourceName;

        //Verifica se o diretório ou arquivo já existe.
        utils.verifyDirAndFile(prefixPath, me.props.resourceName, me.props.structure).then(function(notExists){
          if (notExists === true) {
            mkdirp(me.path, function(err) {
              if(err) me.log(chalk.red(err));
            });
            done();
          } else {
            me.log(notExists);
            verify(askList, askName, me, notExists);
          }
        });

      }.bind(me));

      return prompt;
    }

    var me = this;
    var notExists = true;

    return verify(askList, askName, me, notExists);

  },

  writing: function() {
    //Copia os aquivos html
    var resourceHtml = function(context, options) {
      context.fs.copyTpl(
        context.templatePath('_resource.html'),
        context.destinationPath(context.path + '/' + options.resource_name + '.html'),
        options
      );

      context.fs.copyTpl(
        context.templatePath('_resource-form.html'),
        context.destinationPath(context.path + '/' + options.resource_name + '-form.html'),
        options
      );

      context.fs.copyTpl(
        context.templatePath('_resource-list.html'),
        context.destinationPath(context.path + '/' + options.resource_name + '-list.html'),
        options
      );
    }
    //Copia o arquivo do controller.js
    var resourceController = function(context, options) {
      context.fs.copyTpl(
        context.templatePath('_resource.controller.js'),
        context.destinationPath(context.path + '/' + options.resource_name + '.controller.js'),
        options
      );
    }
    //Copia o arquivo do route.js
    var resourceRoute = function(context, options) {
      context.fs.copyTpl(
        context.templatePath('_resource.route.js'),
        context.destinationPath(context.path + '/' + options.resource_name + '.route.js'),
        options
      );
    }
    //Copia o arquivo do service.js
    var resourceService = function(context, options) {
      context.fs.copyTpl(
        context.templatePath('_resource.service.js'),
        context.destinationPath(context.path + '/' + options.resource_name + '.service.js'),
        options
      );
    }

    //Formata a string para os formatos usados no template
    var options = utils.formatNameResource(this.props.resourceName);
    var structure = this.props.structure;

    switch (structure) {
      case 'html':
        resourceHtml(this, options);
      break;
      case 'controller':
        resourceController(this, options);
      break;
      case 'route':
        resourceRoute(this, options);
      break;
      case 'service':
        resourceService(this, options);
      break;
      default:
        resourceHtml(this, options);
        resourceController(this, options);
        resourceRoute(this, options);
        resourceService(this, options);
    };

  },

  end: function() {
    this.log(chalk.green('\n\n#######      Operação realizada com sucesso!      #######\n\n'));
  }

});

module.exports = PrGenerator;
