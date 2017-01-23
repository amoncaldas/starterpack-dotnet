<?php

use App\User;

class TestCase extends Illuminate\Foundation\Testing\TestCase
{
    /**
     * The base URL to use while testing the application.
     *
     * @var string
     */
    protected $baseUrl = 'http://localhost';

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
    public function createAuthHeader(User $user, $refreshApplication = false)
    {
        $this->authHeaders = [
            'Accept' => 'application/json',
            'Authorization' => 'Bearer ' . \Tymon\JWTAuth\Facades\JWTAuth::fromUser($user)
        ];

        // Strange auth bug, we need to reboot the appilication
        // SEE: https://laracasts.com/discuss/channels/testing/laravel-testig-request-setting-header
        if ($refreshApplication) {
            $this->refreshApplication();
            $this->setUp();
        }

        return $this->authHeaders;
    }

    public function tearDown()
    {
        parent::tearDown();
    }
}
