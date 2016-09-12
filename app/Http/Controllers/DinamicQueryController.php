<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

use OwenIt\Auditing\Log;

use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Input;
use Carbon\Carbon;

class DinamicQueryController extends Controller
{
    public function __construct()
    {
    }


    public function index(Request $request)
    {
        $filters = json_decode($request->filters);

        $baseQuery = \DB::table(str_plural(strtolower($request->model)));

        if( $filters !== null ) {
            foreach($filters as $filter) {
                if( $filter->operator === 'like' )
                    $filter->value = '%' . $filter->value . '%';

                $baseQuery = $baseQuery->where($filter->attribute, $filter->operator, $filter->value);
            }
        }

        $dataQuery = clone $baseQuery;
        $countQuery = clone $baseQuery;

        $data['items'] = $dataQuery
            ->skip(($request->page - 1) * $request->perPage)
            ->take($request->perPage)
            ->get();

        $data['total'] = $countQuery
            ->count();
     

        return $data;
    }

    public function models(Request $request)
    {   
        $models = \Prodeb::modelNames(array("BaseModel.php", "Permission.php", "Role.php"));
        $data = array();

        foreach($models as $model) {
            $tableName = str_plural(strtolower($model));
            $columnWithTypes =  \DB::table('information_schema.columns')->select('column_name as name', 'data_type as type')
                ->where('table_name', $tableName)->get();

            array_push($data, [
                'name' => $model,
                'attributes' => $columnWithTypes
            ]);
        }        

        return $data;
    }    
}
