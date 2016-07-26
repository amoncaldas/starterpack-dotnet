<?php

Route::get('/', function () {
    return File::get(public_path().'/client/index.html');
});

Route::group(['prefix' => 'v1'], function () {
    Route::post('authenticate', 'AuthenticateController@authenticate');

    //authenticated area
    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh']], function () {
        Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');

        Route::group([], function () {
            Route::put('profile', 'UsersController@updateProfile');
        });

        //admin area
        Route::group(['middleware' => ['acl.role:admin']], function () {
            Route::resource('users', 'UsersController', ['except' => ['updateProfile']]);
            Route::resource('roles', 'RolesController');
        });
    });
});
