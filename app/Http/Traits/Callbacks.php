<?php
/**
 * CrudController is a shared base controller that provides a CRUD basis for Laravel applications.
 *
 * @author Jamie Rumbelow <jamie@jamierumbelow.net>
 * @license http://opensource.org/licenses/MIT
 */

namespace App\Http\Traits;

use Illuminate\Http\Request;
use Illuminate\Database\Eloquent\Model;

/**
 * Callback support
 *
 * @internal
 * @uses \Rumbelow\Http\Controllers\CrudController
 * @used-by \Rumbelow\Http\Controllers\CrudController
 */
trait Callbacks
{
    /**
     * The callback registry
     *
     * @var array
     **/
    protected $registeredCallbacks = [
        'beforeAll' => [ 'beforeAll' ],
        'beforeStore' => [ 'beforeStore' ],
        'beforeSearch' => [ 'beforeSearch' ],
        'beforeUpdate' => [ 'beforeUpdate' ],
        'beforeSave' => [ 'beforeSave' ],
        'beforeDestroy' => [ 'beforeDestroy' ],
        'afterStore' => [ 'afterStore' ],
        'afterUpdate' => [ 'afterUpdate' ],
        'afterSave' => [ 'afterSave' ],
        'afterDestroy' => [ 'afterDestroy' ],
        'applyFilters' => ['applyFilters']
    ];

    /**
     * Placeholder callbacks. These are included in the registry by default.
     *
     * @param \Illuminate\Http\Request $request The request object
     * @param \Illuminate\Database\Eloquent\Model $model The model object
     * @return null
     */

    protected function beforeAll(Request $request) { }
    protected function beforeSearch(Request $request, $dataQuery, $countQuery) { }
    protected function beforeStore(Request $request, Model $model) { }
    protected function beforeUpdate(Request $request, Model $model) { }
    protected function beforeSave(Request $request, Model $model) { }
    protected function beforeDestroy(Request $request, Model $model) { }
    protected function afterStore(Request $request, Model $model) { }
    protected function afterUpdate(Request $request, Model $model) { }
    protected function afterSave(Request $request, Model $model) { }
    protected function afterDestroy(Request $request, Model $model) { }
    protected function applyFilters(Request $request, $query) { }

    /**
     * Process callbacks for $eventName
     *
     * @param string $eventName
     **/
    protected function callback($eventName)
    {
        if ( ! isset( $this->registeredCallbacks[$eventName] ) ) {
            throw new \App\Exceptions\CallbackException("Unknown callback: $eventName");
        }

        $parameters = array_slice(func_get_args(), 1);
        $callbacks = $this->registeredCallbacks[$eventName];
        $response = null;

        foreach ( $callbacks as $callback )
        {
            $response = call_user_func_array( is_callable($callback) ? $callback : [ $this, $callback ], $parameters);
        }

        return $response;
    }

    /**
     * Add a callback to the registry
     *
     * @param string $eventName
     * @param callable $callback
     * @return void
     **/
    protected function registerCallback($eventName, $callback)
    {
        if ( isset($this->registeredCallbacks[$eventName]) ) {
            $this->registeredCallbacks[$eventName][] = $callback;
        }
        else {
            $this->registeredCallbacks[$eventName] = [ $callback ];
        }
    }

    /**
     * Does a callback exist for this event?
     *
     * @param string $eventName
     * @return boolean
     **/
    protected function hasCallback($eventName)
    {
        return isset($this->registeredCallbacks[$eventName]);
    }
}
