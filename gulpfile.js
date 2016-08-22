/*eslint strict: 0, */
/*global require*/

var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var sass = require('gulp-sass');
var changed = require('gulp-changed');
var sourcemaps = require('gulp-sourcemaps');
var plumber = require('gulp-plumber');
var ngAnnotate = require('gulp-ng-annotate');
var gulpif = require('gulp-if');
var cleanCSS = require('gulp-clean-css');
var inject = require('gulp-inject');
var series = require('stream-series');
var lazypipe = require('lazypipe');
var browserSync = require('browser-sync');
var shell = require('gulp-shell')
var argv = require('yargs').argv;

var paths = {
  client: 'public/client',
  bower: 'bower_components'
};

paths.app = paths.client + '/app';
paths.destination = paths.client + '/build';
paths.angularScripts = [
  paths.bower + '/deep-diff/index.js',
  paths.bower + '/uri-templates/uri-templates.js',
  paths.bower + '/angular/angular.min.js',
  paths.bower + '/angular-aria/angular-aria.js',
  paths.bower + '/angular-animate/angular-animate.js',
  paths.bower + '/angular-resource/angular-resource.min.js',
  paths.bower + '/satellizer/satellizer.min.js',
  paths.bower + '/angular-i18n/angular-locale_pt-br.js',
  paths.bower + '/angular-input-masks/angular-input-masks-standalone.min.js',
  paths.bower + '/angular-ui-router/release/angular-ui-router.min.js',
  paths.bower + '/angular-material/angular-material.js',
  paths.bower + '/angular-material-data-table/dist/md-data-table.min.js',
  paths.bower + '/angular-model-factory/dist/angular-model-factory.js',
  paths.bower + '/ng-prodeb/dist/ng-prodeb.min.js',
  paths.bower + '/moment/moment.js',
  paths.bower + '/mdPickers/dist/mdPickers.min.js'
];
paths.vendorsScripts = [
  paths.bower + '/lodash/dist/lodash.min.js'
];
paths.scripts = [
  paths.app + '/app.js',
  paths.app + '/app.*.js',
  paths.app + '/**/*.js'
];
paths.styles = [
  paths.bower   + '/ng-prodeb/dist/ng-prodeb.css',
  paths.bower   + '/angular-material-data-table/dist/md-data-table.min.css',
  paths.bower   + '/mdPickers/dist/mdPickers.min.css',
  paths.client  + '/styles/app.scss'
];

var minifierJSChannel = lazypipe()
  .pipe(uglify)
  .pipe(rename, {
    suffix: 'min'
  });

var minifierCSSChannel = lazypipe()
  .pipe(cleanCSS)
  .pipe(rename, {
    suffix: 'min'
  });

function scriptsVendors() {
  return gulp.src(paths.vendorsScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('vendors.js'))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

function scriptsAngular() {
  return gulp.src(paths.angularScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('angular-with-plugins.js'))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

function scriptsApplication() {
  return gulp.src(paths.scripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('application.js'))
    .pipe(ngAnnotate({
      add: true
    }))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

function styles() {
  return gulp.src(paths.styles)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(changed(paths.destination))
    .pipe(sass())
    .pipe(concat('application.css'))
    .pipe(gulpif(argv.production, minifierCSSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

gulp.task('scriptsVendors', scriptsVendors);
gulp.task('scriptsAngular', scriptsAngular);
gulp.task('scriptsApplication', scriptsApplication);
gulp.task('styles', styles);

/**
 * Task to sync the browser with changes in the
 * source code.
 */
gulp.task('browser-sync', function() {
  if (argv.sync && !argv.production) {
    browserSync({
      port: 5005
    });
  }
});

// Rerun the task when a file changes
gulp.task('watch', function() {
  if (!argv.production) {
    gulp.watch(paths.scripts, ['scriptsApplication']).on('change', browserSync.reload);
    gulp.watch(paths.app + '/**/*.html').on('change', browserSync.reload);
    gulp.watch(paths.styles, ['styles']).on('change', browserSync.reload);
  }
});

/**
 * Build js files and inject into index.html
 */
gulp.task('build', function() {
  var cssStream = styles();
  var vendorStream = scriptsVendors();
  var angularStream = scriptsAngular();
  var applicationStream = scriptsApplication();

  return gulp.src(paths.client + '/index.html')
    .pipe(inject(series(
      cssStream,
      vendorStream,
      angularStream,
      applicationStream
    ), {
      ignorePath: 'public',
      addRootSlash: false
    }))
    .pipe(gulp.dest(paths.client));
});

/**
 * Check all .js files using eslint
 * --fix can be passed to fix possible problems
 */
gulp.task('check', shell.task([
  'eslint ' +  paths.app + '/*.js ' + paths.app + '/**/*.js' + ((argv.fix) ? ' --fix' : '')
], {
  ignoreErrors: true
}));

gulp.task('default', ['browser-sync', 'watch', 'build'], function() {});
gulp.task('minifier', ['build-production'], function() {});
