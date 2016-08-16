<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use OwenIt\Auditing\AuditingTrait;

class Project extends Model
{
    protected $table = 'projects';
    protected $fillable = ['name', 'cost'];

    use AuditingTrait;

    protected $auditEnabled  = true;
    protected $historyLimit = 100;
    protected $auditableTypes = ['created', 'saved', 'deleted'];

    /**
    * Retorna os tasks de um determinado projeto.
    */
    public function tasks()
    {

        return $this->hasMany('App\Task');

    }
}
