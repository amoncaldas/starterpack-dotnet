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
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        Log::debug('Carregando os usuários da página: '.$request->page);

        $baseQuery = User::with('roles');

        if(isset($request->name))
            $baseQuery = $baseQuery->where('name', 'like', '%'.$request->name.'%');

        $dataQuery = clone $baseQuery;
        $countQuery = clone $baseQuery;

        $data['items'] = $dataQuery
            ->orderBy('name', 'asc')
            ->skip(($request->page - 1) * $request->perPage)
            ->take($request->perPage)
            ->get();

        $data['total'] = $countQuery
            ->count();

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
            $user->roles = array_pluck($user->roles()->get()->toArray(), 'slug');
        } catch (Exception $e) {
            return Response::json(['error' => 'User already exists.'], HttpResponse::HTTP_CONFLICT);
        }

        return $user;
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

        $user->fill(Input::only('name', 'email'));
        $user->save();
        $user->roles()->sync(Input::only('roles')["roles"]);

        $user->roles = array_pluck($user->roles()->get()->toArray(), 'slug');

        return $user;
    }

    public function destroy($id)
    {
      $user = User::destroy($id);
    }
}
