<?php

use App\User;
use Tymon\JWTAuth\Facades\JWTAuth;

class TestCase extends Illuminate\Foundation\Testing\TestCase
{
    /**
     * The base URL to use while testing the application.
     *
     * @var string
     */
    protected $baseUrl = 'http://localhost';

    protected $adminUserData = [
        'email' => 'admin-base@prodeb.com',
        'password' => 'Prodeb01'
    ];

    protected $normalUserData = [
        'email' => 'normal-base@prodeb.com',
        'password' => 'Prodeb01'
    ];

    /**
     * Creates the application.
     *
     * @return \Illuminate\Foundation\Application
     */
    public function createApplication()
    {
        $app = require __DIR__.'/../bootstrap/app.php';

        $app->loadEnvironmentFrom('.env.testing');

        $app->make(Illuminate\Contracts\Console\Kernel::class)->bootstrap();

        return $app;
    }

    public function setUp()
    {
        parent::setUp();

        Artisan::call('migrate:reset', [
            '--env' => 'testing'
        ]);

        Artisan::call('migrate', [
            '--env' => 'testing',
            '--seed' => true
        ]);
    }

     /**
     * Generate authenication headers
     *
     * @return String
     */
    public function createAuthHeader($token)
    {
        $this->authHeaders = [
            'Accept' => 'application/json',
            'Authorization' => 'Bearer ' . $token
        ];

        // Strange auth bug, we need to reboot the appilication
        // SEE: https://laracasts.com/discuss/channels/testing/laravel-testig-request-setting-header
        $this->refreshApplication();
        $this->setUp();

        return $this->authHeaders;
    }

    public function createAuthHeaderToAdminUser() {
        return $this->createAuthHeader($this->getTokenFromAdminUser());
    }

    public function createAuthHeaderToNormalUser() {
        return $this->createAuthHeader($this->getTokenFromNormalUser());
    }

    public function getTokenFromAdminUser() {
        $admin = User::where('email', $this->adminUserData['email'])->first();
        return JWTAuth::fromUser($admin);
    }

    public function getTokenFromNormalUser() {
        $user = User::where('email', $this->normalUserData['email'])->first();
        return JWTAuth::fromUser($user);
    }

    public function tearDown()
    {
        parent::tearDown();
    }
}
