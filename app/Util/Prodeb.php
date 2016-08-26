<?php

namespace App\Util;

class Prodeb {
    public static function parseDate($date) {
        return \Carbon::parse($date)->timezone(config('app.timezone'));
    }
}
