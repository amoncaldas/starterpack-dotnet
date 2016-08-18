<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Task extends BaseModel
{
    protected $table = 'tasks';
    protected $fillable = [
        'description',
        'done',
        'priority',
        'scheduled_to',
    ];

    public function __construct()
    {
        $this->castAttributes(['scheduled_to' => 'datetime']);
    }

    /**
    * Retorna o projeto de um projeto
    */
    public function project()
    {

        return $this->belongsTo('App\Project');

    }
}
