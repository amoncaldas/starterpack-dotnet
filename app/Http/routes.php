<?php

Route::get('/', function () {
    return File::get(public_path().'/client/index.html');
});

Route::group(['prefix' => 'v1'], function () {
    //public area
    Route::post('authenticate', 'AuthenticateController@authenticate');

    Route::post('password/email', 'PasswordController@postEmail');
    Route::post('password/reset', 'PasswordController@postReset');

    Route::group(['prefix' => 'support'], function () {
        Route::get('langs', 'SupportController@langs');
    });

    //authenticated area
    Route::group(['middleware' => ['jwt.auth', 'jwt.refresh']], function () {
        Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');

        Route::resource('projects', 'ProjectsController');

        Route::put('/tasks/toggleDone', 'TasksController@toggleDone');
        Route::resource('tasks', 'TasksController');

        Route::resource('mails', 'MailsController',
            ['only' => ['store']]);

        Route::group([], function () {
            Route::put('profile', 'UsersController@updateProfile');
        });

        //admin area
        Route::group(['middleware' => ['acl.role:admin']], function () {
            Route::get('audit', 'AuditController@index');
            Route::get('audit/models', 'AuditController@models');

            Route::resource('users', 'UsersController', ['except' => ['updateProfile']]);
            Route::resource('roles', 'RolesController');

            Route::group(['prefix' => 'dinamicQuery'], function () {
                Route::get('/', 'DinamicQueryController@index');
                Route::get('models', 'DinamicQueryController@models');
            });
        });
    });
});

Route::resource('posts', 'PostsController');
