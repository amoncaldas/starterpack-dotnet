process.env.DISABLE_NOTIFIER = true;

var elixir = require('laravel-elixir');
require('laravel-elixir-livereload');

/*
 |--------------------------------------------------------------------------
 | Elixir Asset Management
 |--------------------------------------------------------------------------
 |
 | Elixir provides a clean, fluent API for defining some basic Gulp tasks
 | for your Laravel application. By default, we are compiling the Sass
 | file for our application, as well as publishing vendor resources.
 |
 */

elixir(function (mix) {
  mix.copy('node_modules/toastr/build/toastr.min.css', 'resources/assets/sass/toastr.scss');

  mix
    .sass([
      'toastr.scss',
      'admin.scss',
      'admin-skin-blue.scss',
      'app.scss'
    ], 'public/css/all.css')
    .scripts([
      '../../../node_modules/jquery/dist/jquery.min.js',
      '../../../node_modules/lodash/index.js',
      '../../../node_modules/toastr/build/toastr.min.js',
      '../../../node_modules/bootstrap-sass/assets/javascripts/bootstrap.min.js',
      '../../../node_modules/angular/angular.js',
      '../../../node_modules/restangular/dist/restangular.min.js',
      '../../../node_modules/angular-ui-router/build/angular-ui-router.js',
      '../../../node_modules/satellizer/satellizer.js',
      'src/app.js',
      'src/app.global.js',
      'src/app.config.js',
      'src/app.run.js',
      'src/app.routes.js',
      'src/interceptors/**/*.js',
      'src/services/**/*.js',
      'src/filters/**/*.js',
      'src/controllers/**/*.js',
      'src/directives/**/*.js',
    ], 'public/js/all.js');

  mix
    .version(['css/all.css', 'js/all.js'])
    .livereload([
      'public/**/*',
      'resources/views/**/*'
    ]);

});
