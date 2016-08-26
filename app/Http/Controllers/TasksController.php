<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use App\Task;
use App\Project;
use App\Http\Requests;
use App\Http\Controllers\CrudController;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;
use Carbon\Carbon;

class TasksController extends CrudController
{

    public function __construct()
    {
    }

    protected function getModel()
    {
        return Task::class;
    }

    protected function applyFilters(Request $request, $query) {
        if($request->has('projectId'))
           $query = $query->where('project_id', '=', $request->projectId);

        if($request->has('description'))
            $query = $query->where('description', 'like', '%'.$request->description.'%');

        if($request->has('done'))
           $query = $query->where('done', '=', $request->done);

        if($request->has('dateStart'))
           $query = $query->where('scheduled_to', '>=', Carbon::parse($request->dateEnd));

        if($request->has('dateEnd'))
           $query = $query->where('scheduled_to', '<=', Carbon::parse($request->dateEnd));
    }

    protected function beforeSearch(Request $request, $dataQuery, $countQuery) {
        $dataQuery->orderBy('description', 'asc');
    }

    protected function getValidationRules(Request $request, Model $obj)
    {
        $rules = [
            'description' => 'required|max:256',
            'priority' => 'required|min:1',
            'scheduled_to' => 'required'
        ];

        if ( strpos($request->route()->getName(), 'tasks.update') !== false ) {
            $rules['done'] = 'required';
        }

        return $rules;
    }

    public function beforeStore(Request $request, Model $obj)
    {
        $obj->done = false;
    }

    /**
     * Atualiza o status da tarefa
     */
    public function toggleDone(Request $request)
    {
        $task = Task::find($request->id);

        $this->validate($request, [
            'done' => 'required'
        ]);

        $task->done = $request->done;

        $task->save();

    }
}
