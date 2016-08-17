<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use OwenIt\Auditing\AuditingTrait;

class BaseModel extends Model
{
    use AuditingTrait;

    protected $auditEnabled  = true;
    protected $historyLimit = 50;
    protected $auditableTypes = ['created', 'saved', 'deleted'];

    protected $casts = [
        'created_at' => 'datetime',
        'updated_at' => 'datetime',
        'deleted_at' => 'datetime'
    ];

    protected function castAttributes($moreAttributes = [])
    {
        $this->casts = array_merge($this->casts, $moreAttributes);
    }
}
