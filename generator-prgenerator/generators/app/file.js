'use strict';

var utils = require('./utils');
var pathExists = require('path-exists');
var mkdirp = require('mkdirp');
var chalk = require('chalk');

//Atualiza o arquivo de rota caso já exista
var updateRoute = function(context, resources, options) {
  var path = context.path + '/' + resources[0].toLowerCase() + '.route.js';
  var contentFile = context.fs.read(path);
  var index = contentFile.indexOf('     });');
  var part1 = contentFile.substring(0,index);

  var pattern =
  '     })\n' +
  '      .state(\'' + options.resource_name + '\', {\n' +
  '        url: \'/' + resources[0].toLowerCase() + '/' + options.resource_name + '\',\n' +
  '        templateUrl: Global.clientPath + \'/' + resources[0].toLowerCase() + '/' + options.resource_name + '.html\',\n' +
  '        controller: \'' + options.name_uppercase  + 'Controller as '+ options.alias_controller +'\',\n' +
  '        data: { breadcrumbs: $translate.instant(\'breadcrumbs.'+ resources[0].toLowerCase() +'.'+ options.resource_name +'\'), needProfile: [] }\n' +
  '      });\n\n' +
  '  }\n' +
  '}());';

  context.conflicter.force = true;
  context.write(path, part1 + pattern);

}
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
  var resources = options.resource_name.split(":");
  if (resources.length > 1) {
    options = utils.formatNameResource(resources[1]);
    if (context.props.htmlController) {
      updateRoute(context, resources, options); //Atualiza o arquivo de rota
      resourceHtml(context, options); //copia os html
    }
  }
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

//Faz a chamada das funções de copia
var copyFiles = function(context) {

  //Formata a string para os formatos usados no template
  var options = utils.formatNameResource(context.props.resourceName);
  var structure = context.props.structure;

  switch (structure) {
    case 'html':
      resourceHtml(context, options);
    break;
    case 'controller':
      resourceController(context, options);
    break;
    case 'route':
      resourceRoute(context, options);
    break;
    case 'service':
      resourceService(context, options);
    break;
    default:
      resourceHtml(context, options);
      resourceController(context, options);
      resourceRoute(context, options);
      resourceService(context, options);
  };

}

/**
 * Válida se o diretório ou o arquivo já existe
 * @params {String, String, String} path, value, structure - Caminho do arquivo ou diretório
 * nome do recurso e estrutura a ser criada
 */
function verifyDirAndFile(path, props) {

  var resources = props.resourceName.split(":");
  var response = {path: '', message: '', error: false};

  response.path = path + resources[0] + '/';

  switch(props.structure) {
    case 'html':
      if (resources.length > 1) {
        response.path = response.path + resources[1] + '.html';
      } else {
        response.path = response.path + resources[0] + '.html';
      }
      response.message = 'Diretório do recurso já existe e contém os arquivos html, tente novamente.';
    break;
    case 'controller':
      if (resources.length > 1) {
        response.path = response.path + resources[1] + '.controller.js';
      } else {
        response.path = response.path + resources[0] + '.controller.js';
      }
      response.message = 'Diretório do recurso já existe e contém o controller, tente novamente.';
    break;
    case 'service':
      if (resources.length > 1) {
        response.path = response.path + resources[1] + '.service.js';
      } else {
        response.path = response.path + resources[0] + '.service.js';
      }
      response.message = 'Diretório do recurso já existe e contém o service, tente novamente.';
    break;
    case 'route':
      if (resources.length > 1) {
        response.path = response.path + resources[1] + '.route.js';
      } else {
        response.path = response.path + resources[0] + '.route.js';
      }
      response.message = 'Diretório do recurso já existe e contém o route, tente novamente.';
    break;
    default:
      if (resources.length > 1) {
        response.error = true;
        break;
      }
      response.message = 'Diretório do recurso já existe, tente novamente.';
    break;
  }

  if (response.error) {
    return Promise.resolve(chalk.red('Nome do recurso é inválido para a estrutura escolhida, tente novamente.'));
  } else {
    return pathExists(response.path).then(exists => {
      if (exists) {
        return chalk.red(response.message);
      } else {
        return true;
      }
    });
  }

}

/**
 * Cria o diretorio no caminho informado
 * @params {String, Object} path, props - Caminho do diretorio a ser criado
 * Objeto contendo os dados necessários
 * @return {String} Retorna o caminho do diretório criado
 */
function createDirectory(path, props) {
  var resources = props.resourceName.split(":");
  var dir = path + resources[0];

  mkdirp(dir, function(err) {
    if(err) console.log(chalk.red(err));
  });

  return dir;
}

module.exports = {
  verifyDirAndFile: verifyDirAndFile,
  createDirectory: createDirectory,
  copyFiles: copyFiles
}

