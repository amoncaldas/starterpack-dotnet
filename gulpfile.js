'use strict';

var gulp = require('gulp');
var clean = require('gulp-clean');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var sass = require('gulp-sass');
var changed = require('gulp-changed');
var sourcemaps = require('gulp-sourcemaps');
var minifyCss = require('gulp-minify-css');
var plumber = require('gulp-plumber');

var paths = {
  angularScripts: [
    'bower_components/angular/angular.min.js',
    'bower_components/angular-resource/angular-resource.min.js',
    'bower_components/satellizer/satellizer.min.js',
    'bower_components/ng-dialog/js/ngDialog.min.js',
    'bower_components/angular-i18n/angular-locale_pt-br.js',
    'bower_components/angular-input-masks/angular-input-masks-standalone.min.js',
    'bower_components/angular-ui-router/release/angular-ui-router.min.js',
    'bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js'
  ],
  vendorsScripts: [
    'bower_components/jquery/dist/jquery.min.js',
    'bower_components/lodash/dist/lodash.min.js',
    'bower_components/toastr/toastr.min.js'
  ],
  scripts: [
    'resources/assets/js/app.js',
    'resources/assets/js/app.global.js',
    'resources/assets/js/app.config.js',
    'resources/assets/js/app.run.js',
    'resources/assets/js/app.routes.js',
    'resources/assets/js/interceptors/**/*.js',
    'resources/assets/js/services/**/*.js',
    'resources/assets/js/filters/**/*.js',
    'resources/assets/js/controllers/**/*.js',
    'resources/assets/js/directives/**/*.js',
  ],
  styles: [
    'resources/assets/sass/admin-skin-blue.scss',
    'resources/assets/sass/admin.scss',
    'resources/assets/sass/app.scss',
    'bower_components/toastr/toastr.min.css',
    'bower_components/ng-dialog/css/ngDialog.min.css',
    'bower_components/ng-dialog/css/ngDialog-theme-default.min.css'
  ]
};

gulp.task('clean', function () {
  return gulp.src('build', {read: false})
    .pipe(clean());
});

gulp.task('scripts-vendors', function() {
  return gulp.src(paths.vendorsScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('vendors.js'))
    .pipe(gulp.dest('public/build/js'))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('public/build/js'));
});

gulp.task('scripts-angular', function() {
  return gulp.src(paths.angularScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('angular-with-plugins.js'))
    .pipe(gulp.dest('public/build/js'))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('public/build/js'));
});

gulp.task('scripts', function() {
  return gulp.src(paths.scripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat('application.js'))
    .pipe(gulp.dest('public/build/js'))
    // .pipe(rename('all.min.js'))
    // .pipe(uglify())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('public/build/js'));
});

gulp.task('styles', function () {
  return gulp.src(paths.styles)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(changed('public/build/css'))
    .pipe(sass())
    .pipe(concat('all.min.css'))
    .pipe(minifyCss())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('public/build/css'));
});

// Rerun the task when a file changes
gulp.task('watch', function() {
  gulp.watch(paths.scripts, ['scripts']);
  gulp.watch(paths.styles, ['styles']);
});

gulp.task('default', ['watch', 'scripts-vendors', 'scripts-angular', 'scripts', 'styles'], function(){});
