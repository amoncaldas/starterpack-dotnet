<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use App\Task;
use App\Project;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;

class TasksController extends Controller
{

    public function __construct()
    {
    }

    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        $baseQuery = new Task;

        if($request->has('projectId'))
           $baseQuery = $baseQuery->where('project_id', '=', $request->projectId);

        if($request->has('description'))
            $baseQuery = $baseQuery->where('description', 'like', '%'.$request->description.'%');

        $dataQuery = clone $baseQuery;
        $countQuery = clone $baseQuery;

        $data['items'] = $dataQuery
            ->orderBy('description', 'asc')
            ->skip(($request->page - 1) * $request->perPage)
            ->take($request->perPage)
            ->get();

        $data['total'] = $countQuery
            ->count();

        return $data;
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        //
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
            'description' => 'required|max:256',
            'priority' => 'required|min:1',
            'scheduled_to' => 'required'
        ]);

        $task = new Task($request->all());
        $task->done = false;
        $task->project()->associate(new Project($request->project));

        try {
            $task->save();
        } catch (Exception $e) {
            return Response::json(['error' => 'Task already exists.'], HttpResponse::HTTP_CONFLICT);
        }

        return $task;
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit($id)
    {
        //
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
        $task = Task::find($id);

        $this->validate($request, [
            'description' => 'required|max:256',
            'priority' => 'required|min:1',
            'scheduled_to' => 'required',
            'done' => 'required'
        ]);

        $task->fill($request->all());
        $task->project()->associate(new Project($request->project));
        $task->save();

        return $task;
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        $task = Task::destroy($id);
    }
}
