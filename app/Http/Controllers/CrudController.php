<?php
/**
 * CrudController is a shared base controller that provides a CRUD basis for Laravel applications.
 *
 * @author Jamie Rumbelow <jamie@jamierumbelow.net>
 * @license http://opensource.org/licenses/MIT
 */

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Database\Eloquent\Model;

use App\Http\Requests;
use App\Http\Controllers\Controller;

use App\Http\Traits\Callbacks,
    App\Http\Traits\Actions;

abstract class CrudController extends Controller
{
    use Callbacks, Actions;
    /**
     * The array of booted controllers.
     *
     * @var array
     */
    protected static $booted = [];

    /**
     * The current action being processed by the controller.
     *
     * @var string
     **/
    protected $currentAction;

    /**
     * undocumented class variable
     *
     * @var string
     **/
    protected $alertSuccessKey = 'alerts.success';

    /**
     * Class constructor.
     */
    public function __construct(Request $request)
    {
        static::bootIfNotBooted();

        parent::__construct();

        if ( $request->route() ) {
            $this->currentAction = explode('@', $request->route()->getActionName())[1];
        }
    }

    /**
     * Get the model class.
     *
     * @return string
     */
    abstract protected function getModel();

    /**
     * Get the model class.
     *
     * @return string
     */
    abstract protected function getValidationRules(Request $request, Model $obj);

    /**
     * Booting Mechanism (taken from \Illuminate\Database\Eloquent\Model)
     *
     * @author Taylor Otwell
     * @author Jamie Rumbelow
     * -----------------------------------------------------------------------------------------------------------------------------
     */

    /**
     * Check if the controller needs to be booted and if so, do it.
     *
     * @return void
     */
    protected function bootIfNotBooted()
    {
        if (! isset(static::$booted[static::class])) {
            static::$booted[static::class] = true;
            static::boot( $this );
        }
    }

    /**
     * The "booting" method of the controller.
     *
     * @param \Rumbelow\Http\Controllers\CrudController $instance The instance being booted
     * @return void
     */
    protected static function boot( $instance )
    {
        static::bootTraits($instance);
    }

    /**
     * Boot all of the bootable traits on the controller.
     *
     * @param \Rumbelow\Http\Controllers\CrudController $instance The instance being booted
     * @return void
     */
    protected static function bootTraits( $instance )
    {
        $class = static::class;

        foreach (class_uses_recursive($class) as $trait) {
            if (method_exists($class, $method = 'boot'.class_basename($trait))) {
                forward_static_call_array([$class, $method], [$instance]);
            }
        }
    }
}
