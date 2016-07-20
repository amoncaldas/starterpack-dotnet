<?php

Route::get('/', function () {
    return File::get(public_path().'/client/index.html');
});

Route::group(['prefix' => 'v1'], function () {
    Route::post('authenticate', 'AuthenticateController@authenticate');
    Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');
    

    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh']], function () {
        Route::group([], function () {
            Route::put('profile', 'UsersController@updateProfile');
        });

        //admin area
        Route::group(['middleware' => ['acl.role:admin']], function () {
            Route::resource('users', 'UsersController', ['only' => ['index', 'store', 'update', 'show', 'destroy']]);
            Route::resource('roles', 'RolesController', ['only' => ['index', 'store', 'update']]);
        });
    });
});

// Using different syntax for Blade to avoid conflicts with AngularJS.
// You are well-advised to go without any Blade at all.
Blade::setContentTags('<%', '%>'); // For variables and all things Blade.
Blade::setEscapedContentTags('<%%', '%%>'); // For escaped data.
