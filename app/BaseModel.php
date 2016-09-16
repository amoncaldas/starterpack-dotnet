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

    protected $casts = [];

    protected $dateFormat = 'Y-m-d H:i:sO';

    protected function addCast($moreAttributes = [])
    {
        $this->casts = array_merge($this->casts, $moreAttributes);
    }

    public function setAttribute($key, $value)
    {
        if (in_array($key, $this->dates) && is_string($value)) {
            $this->attributes[$key] = \Prodeb::parseDate($value);
        } else {
            parent::setAttribute($key, $value);
        }
    }
}
