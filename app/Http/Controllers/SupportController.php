<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use OwenIt\Auditing\Log;

use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;
use Carbon\Carbon;

class SupportController extends Controller
{
    public function __construct()
    {
    }


    public function langs(Request $request)
    {
        return [
            'attributes' => trans('validation.attributes')
        ];
    }
}
