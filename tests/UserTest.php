<?php

use Illuminate\Foundation\Testing\WithoutMiddleware;
use Illuminate\Foundation\Testing\DatabaseMigrations;
use Illuminate\Foundation\Testing\DatabaseTransactions;

use Tymon\JWTAuth\Facades\JWTAuth;
use App\User;

class UserTest extends TestCase
{

    public function testList()
    {
        $params = $this->createAuthHeaderToAdminUser();

        $response = $this->get('/v1/users', $params);

        $response->assertResponseStatus(200);

        $response->seeJsonStructure(['*' => [
            'email', 'name', 'roles'
        ]]);
    }

    public function testListWithPaginate()
    {
        $params = $this->createAuthHeaderToAdminUser();

        $query = [
            'perPage' => 1,
            'page' => 1
        ];

        $response = $this->get('/v1/users?' . http_build_query($query), $params);

        $response->seeJsonStructure([
            'total', 'items' => [
                '*' => ['email', 'name', 'roles']
            ]
        ]);
    }

}
