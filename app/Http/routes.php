<?php

Route::get('/', function () {
    return view('index');
});

Route::group(['prefix' => 'v1'], function () {
    Route::post('authenticate', 'AuthenticateController@authenticate');
    Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');

    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh']], function () {
        Route::put('profile', 'UsersController@updateProfile');
    });

    //admin area
    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh', 'acl.role:admin']], function () {
        Route::resource('users', 'UsersController', ['only' => ['index', 'store', 'update', 'show', 'destroy']]);
        Route::resource('roles', 'RolesController', ['only' => ['index', 'store', 'update']]);
    });
});

Route::get(
    '/templates/{templatePath?}',
    function($templatePath = null) {
      return File::get(join('/', array_filter([Config::get('view.paths')[0], 'templates', $templatePath])));
    }
)->where('templatePath', '(.*)');

// Using different syntax for Blade to avoid conflicts with AngularJS.
// You are well-advised to go without any Blade at all.
Blade::setContentTags('<%', '%>'); // For variables and all things Blade.
Blade::setEscapedContentTags('<%%', '%%>'); // For escaped data.
