<?php

namespace App;

use App\BaseModel;

use Illuminate\Database\Eloquent\Model;

class Post extends BaseModel
{
    /**
     * The database table used by the model.
     *
     * @var string
     */
    protected $table = 'Post';

    /**
     * Attributes that should be mass-assignable.
     *
     * @var array
     */
    protected $fillable = ['name'];

    
}
