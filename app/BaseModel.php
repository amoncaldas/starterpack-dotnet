<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use OwenIt\Auditing\AuditingTrait;

use Carbon\Carbon;

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

    protected $dates = [];

    protected function castAttributes($moreAttributes = [])
    {
        $this->casts = array_merge($this->casts, $moreAttributes);
    }

    public function setAttribute($key, $value)
    {
        if (in_array($key, $this->dates)) {
            $this->attributes[$key] = new Carbon($value);
        } else {
            parent::setAttribute($key, $value);
        }
    }
}
