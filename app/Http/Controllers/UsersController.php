<?php

namespace App\Http\Controllers;

use App\User;

use Hash;
use Log;

use Illuminate\Http\Request;

use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;

class UsersController extends Controller
{
    public function __construct()
    {
    }

    /**
     * Display a listing of the resource.
     * If page info is passed get a paginated list instead
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        $baseQuery = User::with('roles');

        if($request->has('name'))
            $baseQuery = $baseQuery->where('name', 'like', '%'.$request->name.'%');

        $dataQuery = clone $baseQuery;
        $dataQuery = $dataQuery->orderBy('name', 'asc');

        if( $request->has('perPage', 'page') ) {
            $countQuery = clone $baseQuery;
            $perPage = $request->input('perPage', $this->perPage);

            $data['items'] = $dataQuery
                ->skip(($request->page - 1) * $perPage)
                ->take($perPage)
                ->get();

            $data['total'] = $countQuery
                ->count();
        } else {
            $data = $dataQuery->get();
        }

        return $data;
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $this->validate($request, [
            'name' => 'required|max:255',
            'email' => 'required|email|max:255|unique:users'
        ]);

        $user = new User;
        $user->fill(Input::only('name', 'email'));
        $user->password = bcrypt(str_random(10));

        try {
            $user->save();
            $user->roles()->sync(Input::only('roles')["roles"]);

            $newRoles = array_pluck($user->roles()->get()->toArray(), 'slug');
            $this->auditRoles($user, [], $newRoles);

            $user->roles = $newRoles;
        } catch (Exception $e) {
            return Response::json(['error' => 'User already exists.'], HttpResponse::HTTP_CONFLICT);
        }

        return $user;
    }

    protected function auditRoles($user, $oldRoles, $newRoles) {
        sort($newRoles);
        sort($oldRoles);

        if( $oldRoles !== $newRoles )
        {
            $log = [
                'new_value' => $newRoles,
                'old_value' => $oldRoles
            ];

            $user->audit($log, 'updated');
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        $user = User::findOrFail($id);
        return $user;
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

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
        $user = User::find($id);

        $this->validate($request, [
            'name' => 'required|max:255',
            'email' => 'required|email|max:255|unique:users,email,'.$user->id,
        ]);

        $oldRoles = array_pluck($user->roles()->get()->toArray(), 'slug');

        $user->fill(Input::only('name', 'email'));
        $user->save();
        $user->roles()->sync(Input::only('roles')["roles"]);

        $newRoles = array_pluck($user->roles()->get()->toArray(), 'slug');

        $this->auditRoles($user, $oldRoles, $newRoles);

        $user->roles = $newRoles;

        return $user;
    }

    public function destroy($id)
    {
      $user = User::destroy($id);
    }
}
