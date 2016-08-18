<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use OwenIt\Auditing\AuditingTrait;

class Project extends BaseModel
{
    protected $table = 'projects';
    protected $fillable = ['name', 'cost'];

    public function __construct()
    {
        $this->castAttributes(['cost' => 'real']);
    }

    /**
    * Retorna os tasks de um determinado projeto.
    */
    public function tasks()
    {

        return $this->hasMany(Task::class);

    }
}
