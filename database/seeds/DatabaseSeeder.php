<?php

use Illuminate\Database\Seeder;
use Illuminate\Database\Eloquent\Model;
use App\User;
use App\Role;

class DatabaseSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        Model::unguard();

        DB::table('users')->delete();
        DB::table('roles')->delete();
        DB::table('role_user')->delete();

        $users = array(
                ['name' => 'Admin', 'email' => 'admin-base@prodeb.com', 'password' => Hash::make('Prodeb01')],
                ['name' => 'Normal', 'email' => 'normal-base@prodeb.com', 'password' => Hash::make('Prodeb01')],
                ['name' => 'User Test Updated', 'email' => 'updated-base@prodeb.com', 'password' => Hash::make('Prodeb01')],
                ['name' => 'Usuário 2', 'email' => 'usuario2-base@prodeb.com', 'password' => Hash::make('secret')],
                ['name' => 'Usuário 3', 'email' => 'usuario3-base@prodeb.com', 'password' => Hash::make('secret')],
        );

        // Loop through each user above and create the record for them in the database
        foreach ($users as $user)
        {
            User::create($user);
        }

        $role = new Role;
        $role->title = 'Admin';
        $role->slug = 'admin';
        $role->save();

        User::where('email', 'admin-base@prodeb.com')->first()->roles()->attach($role->id);

        Model::reguard();
    }
}
