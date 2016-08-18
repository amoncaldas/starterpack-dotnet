<?php

namespace App;

use App\Project;
use Illuminate\Database\Eloquent\Model;
use Carbon\Carbon;

class Task extends BaseModel
{
    protected $table = 'tasks';
    protected $fillable = [
        'description',
        'done',
        'priority',
        'scheduled_to',
        'project_id'
    ];

    protected $dates = ['scheduled_to'];

    /**
    * Retorna o projeto de um projeto
    */
    public function project()
    {
        return $this->belongsTo(Project::class);
    }
}
