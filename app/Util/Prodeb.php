<?php

namespace App\Util;

class Prodeb {
    public static function parseDate($date) {
        return \Carbon::parse($date)->timezone(config('app.timezone'));
    }

    public static function modelNames($ignoredModels = array()) {
        $models = array();
        $path = app_path();
        $files = scandir($path);

        foreach($files as $file) {
            //skip all dirs and ignoredModels
            if ($file === '.' || $file === '..' || is_dir($path . '/' . $file) || in_array($file, $ignoredModels)) continue;
            
            $models[] = preg_replace('/\.php$/', '', $file);
        }

        return $models;   
    }
}
