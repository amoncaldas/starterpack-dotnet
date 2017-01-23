<?php

use Illuminate\Foundation\Testing\WithoutMiddleware;
use Illuminate\Foundation\Testing\DatabaseMigrations;
use Illuminate\Foundation\Testing\DatabaseTransactions;

use Tymon\JWTAuth\Facades\JWTAuth;
use App\User;

class UserTest extends TestCase
{

    public function testLoginInvalidCredentials()
    {
        $this->post('/v1/authenticate', [
            'email' => 'invalidacredentials@prodeb.com',
            'password' => 'Prodeb01'
        ])
        ->assertResponseStatus(401);
    }

    public function testLoginValidCredentials()
    {
        $response = $this->post('/v1/authenticate', [
            'email' => 'admin-base@prodeb.com',
            'password' => 'Prodeb01'
        ]);

        $response->assertResponseOk();
        $response->seeJson();
    }

    public function testGetAuthenticatedUserData() {
        $admin = User::where('email', 'admin-base@prodeb.com')->first();

        $token = JWTAuth::fromUser($admin);

        Log::info($admin);

        $this->get('/v1/users', $this->createAuthHeader($admin, true));
    }
}
