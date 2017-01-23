<?php

use Illuminate\Foundation\Testing\WithoutMiddleware;
use Illuminate\Foundation\Testing\DatabaseMigrations;
use Illuminate\Foundation\Testing\DatabaseTransactions;

use Tymon\JWTAuth\Facades\JWTAuth;
use App\User;

class LoginTest extends TestCase
{

    public function testLoginInvalidCredentials()
    {
        $this->post('/v1/authenticate', [
            'email' => 'invalidacredentials@prodeb.com',
            'password' => 'iu33j198uy8'
        ])
        ->assertResponseStatus(401);
    }

    public function testLoginValidCredentials()
    {
        $response = $this->post('/v1/authenticate', $this->adminUserData);

        $response->assertResponseOk();
        $response->seeJsonStructure(['token']);
    }

    public function testGetAuthenticatedUserData() {
        $response = $this->get('/v1/authenticate/user', $this->createAuthHeaderToAdminUser());

        $response->seeJsonStructure(['user' => [
            'email', 'name', 'roles'
        ]]);
    }
}
