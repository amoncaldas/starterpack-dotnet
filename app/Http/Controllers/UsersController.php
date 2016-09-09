<?php

namespace App\Http\Controllers;

use App\User;

use Mail;
use Hash;
use Log;

use Illuminate\Http\Request;

use App\Http\Requests;
use App\Http\Controllers\CrudController;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;

class UsersController extends CrudController
{
    public function __construct()
    {
    }

    protected function getModel()
    {
        return User::class;
    }

    protected function applyFilters(Request $request, $query)
    {
        $query = $query->with('roles');

        if($request->has('name'))
            $query = $query->where('name', 'like', '%'.$request->name.'%');

        if($request->has('email'))
            $query = $query->where('email', 'like', '%'.$request->email.'%');

        if($request->has('nameOrEmail'))
            $query = $query
                ->where('name', 'like', '%'.$request->nameOrEmail.'%')
                ->orWhere('email', 'like', '%'.$request->nameOrEmail.'%');

        if($request->has('notUsers'))
            $query = $query->whereNotIn('id', explode(',', $request->notUsers));
    }

    protected function beforeSearch(Request $request, $dataQuery, $countQuery) {
        $dataQuery->orderBy('name', 'asc');
    }

    protected function getValidationRules(Request $request, Model $obj)
    {
        $rules = [
            'name' => 'required|max:255',
            'email' => 'required|email|max:255|unique:users'
        ];

        if ( strpos($request->route()->getName(), 'users.update') !== false ) {
            $rules['email'] = 'required|email|max:255|unique:users,email,'.$obj->id;
        }

        return $rules;
    }

    protected function beforeStore(Request $request, Model $obj)
    {
        $obj->passwordConteiner = str_random(10);
        $obj->password = bcrypt($obj->passwordConteiner);
    }

    protected function beforeUpdate(Request $request, Model $obj)
    {
        $request->merge(array('oldRoles' => array_pluck($obj->roles()->get()->toArray(), 'slug')));
    }

    //After Store and Update
    protected function afterSave(Request $request, Model $obj)
    {
        $obj->roles()->sync(Input::only('roles')["roles"]);

        $newRoles = array_pluck($obj->roles()->get()->toArray(), 'slug');
        $this->auditRoles($obj, $request->oldRoles, $newRoles);

        $obj->roles = $newRoles;

        Mail::send('mails.confirmRegister',
            ['user' => $obj, 'url' => config('app.url'), 'appName' => config('app.app_name')], function($message) use ($obj) {
                $message->from(config('mail.from.address'), config('mail.from.name'));
                $message->to($obj->email);
                $message->subject("Confirmação de cadastro");
        });
    }

    protected function auditRoles($user, $oldRoles, $newRoles) {
        if( !isset($oldRoles) ) $oldRoles = [];

        sort($newRoles);
        sort($oldRoles);

        if( $oldRoles !== $newRoles )
        {
            $log = [
                'new_value' => [ 'roles' => $newRoles ],
                'old_value' => [ 'roles' => $oldRoles ]
            ];

            $user->audit($log, 'updated');
        }
    }

    /**
     * Atualiza os dados do usuário logado
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function updateProfile(Request $request)
    {
        $user = Auth::user();
        $this->validate($request, [
            'name' => 'required|max:255',
            'email' => 'required|email|max:255|unique:users,email,'.$user->id,
            'password' => 'confirmed|min:6',
        ]);

        $user->fill(Input::only('name', 'email'));
        if($request->has('password'))
            $user->password = Hash::make($request->password);

        $user->save();

        //get the roles to return do view
        $user->roles = array_pluck($user->roles()->get()->toArray(), 'slug');

        return $user;
    }
}
