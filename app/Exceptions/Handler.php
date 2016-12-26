<?php

namespace App\Exceptions;

use Exception;
use Log;
use Illuminate\Database\Eloquent\ModelNotFoundException;
use Symfony\Component\HttpKernel\Exception\HttpException;
use Symfony\Component\HttpKernel\Exception\NotFoundHttpException;
use Symfony\Component\HttpKernel\Exception\BadRequestHttpException;
use Symfony\Component\HttpKernel\Exception\UnauthorizedHttpException;
use Tymon\JWTAuth\Exceptions\Exceptions;
use App\Exception\BusinessException;
use Illuminate\Database\QueryException;
use Illuminate\Foundation\Exceptions\Handler as ExceptionHandler;
use Tymon\JWTAuth\Exceptions\TokenExpiredException;

class Handler extends ExceptionHandler
{
    /**
     * A list of the exception types that should not be reported.
     *
     * @var array
     */
    protected $dontReport = [
        HttpException::class,
        ModelNotFoundException::class,
    ];

    /**
     * Report or log an exception.
     *
     * This is a great spot to send exceptions to Sentry, Bugsnag, etc.
     *
     * @param  \Exception  $e
     * @return void
     */
    public function report(Exception $e)
    {
        return parent::report($e);
    }

    /**
     * Render an exception into an HTTP response.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Exception  $e
     * @return \Illuminate\Http\Response
     */
    public function render($request, Exception $e)
    {
        $headers = [
            'Access-Control-Allow-Origin' => '*',
            'Access-Control-Allow-Methods' => 'POST, GET, OPTIONS, PUT, DELETE',
            'Access-Control-Allow-Headers' => '*'
        ];

        //Exceptions relativas ao token

        if ($e instanceof TokenExpiredException) {
            return response()->json(['error' =>'token_expired'], $e->getStatusCode(), $headers);
        }

        if ($e instanceof UnauthorizedHttpException) {
            return response()->json(['error' =>'token_expired'], $e->getStatusCode(), $headers);
        }

        if ($e instanceof TokenInvalidException) {
            return response()->json(['error' =>'token_invalid'], $e->getStatusCode(), $headers);
        }

        if ($e instanceof JWTException) {
           return response()->json(['error' =>'token_absent'], $e->getStatusCode(), $headers);
        }

        if ($e instanceof BadRequestHttpException && $e->getMessage() == "Token not provided") {
            return response()->json(['error' => 'token_not_provided'], $e->getStatusCode(), $headers);
        }

       if ($e instanceof BadRequestHttpException && $e->getMessage() == "Token not provided") {
            return response()->json(['error' => 'token_not_provided'], $e->getStatusCode(), $headers);
        }


        //Exceptions especificas com tratamento especial

        $response = null;

        if ($e instanceof ModelNotFoundException) {
            $response = response()->json(['error' =>'model_not_found'], 404, $headers);
        }

       if ($e instanceof BusinessException) {
            $response = response()->json(['error' => $e->getMessage()], 400, $headers);
        }

        if ($e instanceof QueryException) {
            Log::debug('Erro no acesso ao bando de dados: '.$e->getMessage());

            if (strpos($e->getMessage(), 'not-null') !== false) {
                return response()->json(['error' => 'messages.notNullError'], 400, $headers);
            }
        }

        // Demais exceptions sem tratamento especifico

        if( $response === null ) {
            if (config('app.debug')) {
                $content = ['error' => $e->getMessage()];
            } else {
                $content = ['error' => 'messages.internalError'];
            }

            $response = response()
                ->json($content,  method_exists($e, 'getStatusCode') ? $e->getStatusCode() : 500, $headers);
        }

        //Dá um refresh no token caso o mesmo exista para anexar a resposta
        try {
            $token = \JWTAuth::parseToken()->refresh();

            if( $token !== null )
                $response = $response->header('Authorization', 'Bearer '. $token);
        } catch (Exception $ex) {
            Log::debug('Request without token');
        }

        return $response;
    }
}
