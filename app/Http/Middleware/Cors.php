<?php

namespace App\Http\Middleware;

use Closure;

class Cors
{
    /**
     * Middleware que trata o CORS.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
        // Pega todos os cabeçalhos necessários para o CORS
        $headers = \Prodeb::getCORSHeaders();

        //Trata as requisições OPTIONS preflight
        if ($request->isMethod('OPTIONS')) {
            return \Response::json('{"method":"OPTIONS"}', 200, $headers);
        }

        $response = $next($request);

        //Adiciona os cabeçalhos na resposta
        foreach ($headers as $key => $value) {
            $response->header($key, $value);
        }

        return $response;
    }

}
