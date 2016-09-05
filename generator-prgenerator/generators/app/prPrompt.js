'use strict';

var chalk = require('chalk');
var utils = require('./utils');

function mountQuestions() {
  var questions = {};

  questions.structure = {
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
  questions.name = {
    type: 'input',
    name: 'resourceName',
    message: chalk.yellow('Digite o nome do recurso?'),
    validate: function (value) {
      return utils.verifyInput(value);
    }
  };
  questions.htmlConfirm = {
    type: 'list',
    name: 'htmlController',
    message: chalk.yellow('Deseja criar os arquivos html correspondentes?'),
    choices: [{name: 'Sim', value: true}, {name: 'Não', value: false}]
  };

  return questions;

}

module.exports = {
  mountQuestions: mountQuestions
}
