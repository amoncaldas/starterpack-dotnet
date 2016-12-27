<?php

namespace App;

use App\Role;
use Illuminate\Database\Eloquent\Model;

/**
 * App\Permission
 *
 * @property int $id
 * @property string $title
 * @property string $slug
 * @property string $description
 * @property-read \Illuminate\Database\Eloquent\Collection|\App\Role[] $roles
 * @method static \Illuminate\Database\Query\Builder|\App\Permission whereId($value)
 * @method static \Illuminate\Database\Query\Builder|\App\Permission whereTitle($value)
 * @method static \Illuminate\Database\Query\Builder|\App\Permission whereSlug($value)
 * @method static \Illuminate\Database\Query\Builder|\App\Permission whereDescription($value)
 */
class Permission extends Model
{
    /**
     * The database table used by the model.
     *
     * @var string
     */
    protected $table = 'permissions';
    public $timestamps = false;

    /*
    |--------------------------------------------------------------------------
    | Relationship Methods
    |--------------------------------------------------------------------------
    */

    /**
     * many-to-many relationship method
     *
     * @return QueryBuilder
     */
    public function roles()
    {
        return $this->belongsToMany(Role::class);
    }
}
