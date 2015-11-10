<?php

Route::get('/', function () {
    return view('index');
});

Route::group(['prefix' => 'api'], function()
{
    Route::resource('authenticate', 'AuthenticateController', ['only' => ['index']]);
    Route::post('authenticate', 'AuthenticateController@authenticate');
    Route::get('authenticate/user', 'AuthenticateController@getAuthenticatedUser');
    Route::get('authenticate/teste', 'AuthenticateController@teste');
});

Route::get(
    '/templates/{templatePath?}',
    function($templatePath = null) {
      return File::get(join('/', array_filter([Config::get('view.paths')[0], 'templates', $templatePath])));
    }
)->where('templatePath', '(.*)');;

// Using different syntax for Blade to avoid conflicts with AngularJS.
// You are well-advised to go without any Blade at all.
Blade::setContentTags('<%', '%>'); // For variables and all things Blade.
Blade::setEscapedContentTags('<%%', '%%>'); // For escaped data.
