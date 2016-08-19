<?php

Route::get('/', function () {
    return File::get(public_path().'/client/index.html');
});

Route::group(['prefix' => 'v1'], function () {
    //public area
    Route::post('authenticate', 'AuthenticateController@authenticate');

    //authenticated area
    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh']], function () {
        Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');

        Route::resource('projects', 'ProjectsController');

        Route::group(['prefix' => 'tasks'], function () {
            Route::resource('/', 'TasksController');
            Route::put('/toggleDone', 'TasksController@toggleDone');
        });

        Route::group([], function () {
            Route::put('profile', 'UsersController@updateProfile');
        });

        //admin area
        Route::group(['middleware' => ['acl.role:admin']], function () {
            Route::get('audit', 'AuditController@index');
            Route::resource('users', 'UsersController', ['except' => ['updateProfile']]);
            Route::resource('roles', 'RolesController');
        });
    });
});
