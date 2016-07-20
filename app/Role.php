<?php

namespace App;

use App\User;
use App\Permission;
use Illuminate\Database\Eloquent\Model;

class Role extends Model
{
    /**
     * The database table used by the model.
     *
     * @var string
     */
    protected $table = 'roles';
    public $timestamps = false;

    /*
    |--------------------------------------------------------------------------
    | Relationship Methods
    |--------------------------------------------------------------------------
    */

    /**
     * many-to-many relationship method.
     *
     * @return QueryBuilder
     */
    public function users()
    {
        return $this->belongsToMany(User::class);
    }

    /**
     * many-to-many relationship method.
     *
     * @return QueryBuilder
     */
    public function permissions()
    {
        return $this->belongsToMany(Permission::class);
    }
}
