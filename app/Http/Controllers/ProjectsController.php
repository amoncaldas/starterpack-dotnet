<?php

namespace App\Http\Controllers;

use App\Project;

use Hash;
use Log;

use Illuminate\Http\Request;

use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Input;

class ProjectsController extends Controller
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
        $baseQuery = Project::with('tasks');

        if($request->has('name'))
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
            'name' => 'required|max:100|unique:projects',
            'cost' => 'required|min:1'
        ]);

        $project = new Project;
        $project->fill(Input::only('name', 'cost'));

        try {
            $project->save();
            $project->tasks()->sync(Input::only('tasks')["tasks"]);
        } catch (Exception $e) {
            return Response::json(['error' => 'Project already exists.'], HttpResponse::HTTP_CONFLICT);
        }

        return $project;
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        $project = Project::findOrFail($id);
        return $project;
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
        $project = Project::find($id);

        $this->validate($request, [
            'name' => 'required|max:255|unique:projects,name,'.$project->id,
            'cost' => 'required|min:1'
        ]);

        $project->fill(Input::only('name', 'cost'));
        $project->save();

        return $project;
    }

    public function destroy($id)
    {
      $project = Project::destroy($id);
    }
}
