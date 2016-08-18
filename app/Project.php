<?php

namespace App;

use App\Task;

use Illuminate\Database\Eloquent\Model;
use OwenIt\Auditing\AuditingTrait;

class Project extends BaseModel
{
    protected $table = 'projects';
    protected $fillable = ['id', 'name', 'cost'];

    public function __construct($attributes = array())
    {
        parent::__construct($attributes);

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
