<?php

namespace App\Http\Controllers;

use App\Post;

use Hash;
use Log;

use Illuminate\Http\Request;

use App\Http\Requests;
use App\Http\Controllers\CrudController;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Facades\Input;


class PostsController extends CrudController
{
    public function __construct()
    {
    }

    protected function getModel()
    {
        return Post::class;
    }

    protected function applyFilters(Request $request, $query) {
        $query = $query->with('{relationships}');

        if($request->has('{atributes}')) {
            $query = $query->where('{atributes}', 'like', '%'.$request->{atributes}.'%');
        }
    }

    protected function beforeSearch(Request $request, $dataQuery, $countQuery) {
        $dataQuery->orderBy('{atributes}', 'asc');
    }

    protected function getValidationRules(Request $request, Model $obj)
    {
        $rules = [
            '{atributes}' => 'required|max:100|unique:Posts',
        ];

        if ( strpos($request->route()->get{atributes}(), 'Posts.update') !== false ) {
            $rules['{atributes}'] = 'required|max:255|unique:Posts,name,'.$obj->id;
        }

        return $rules;
    }
}
