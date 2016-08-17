<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Task extends Model
{
    protected $table = 'tasks';

    /**
    * Retorna o projeto de um projeto
    */
    public function project()
    {

        return $this->belongsTo('App\Project');

    }
}
