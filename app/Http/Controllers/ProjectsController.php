<?php

namespace App\Http\Controllers;

use App\Project;

use Hash;
use Log;

use Illuminate\Http\Request;

use App\Http\Requests;
use App\Http\Controllers\CrudController;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Input;

class ProjectsController extends CrudController
{
    public function __construct()
    {
    }

    protected function getModel()
    {
        return Project::class;
    }

    protected function applyFilters(Request $request, $query) {
        $query = $query->with('tasks');

        if($request->has('name'))
            $query = $query->where('name', 'like', '%'.$request->name.'%');

        return $query;
    }

    protected function getValidationRules(Request $request, Model $obj)
    {
        $rules = [
            'name' => 'required|max:100|unique:projects',
            'cost' => 'required|min:1'
        ];

        if ( $request->route()->getName() === 'projects.update' ) {
            $rules['name'] = 'required|max:255|unique:projects,name,'.$obj->id;
        }

        return $rules;
    }
}
