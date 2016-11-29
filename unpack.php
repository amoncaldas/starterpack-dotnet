<?php

$pkgName = htmlspecialchars($_GET['pkgName']); //'MaisDigital.zip'; // O mesmo nome do PKG_NAME mas com a extensão zip
$url = htmlspecialchars($_GET['url']); //'http://homologa.maisdigital.ba.gov.br';
$dir = htmlspecialchars($_GET['dir']); //'./';

try {

    //echo $pkgName . " - " . $url .  " - " . $dir;
    $file=scandir($dir);
    $cont = count($file);
    $conteudo = '';

    for ($i=0; $i < $cont; $i++) {
        //echo $file[$i] . " ";
        if ($file[$i] != $pkgName && $file[$i] != 'unpack.php'
                && $file[$i] != '.' && $file[$i] != '..') {
            $conteudo .= $file[$i] . " ";
        }
    }

    if ($cont > 4) {
        echo shell_exec('rm -rf ' . $conteudo);
    }

    $zip = new ZipArchive();
    $open = $zip->open($pkgName);
    if( $open === true){

        $zip->extractTo($dir);

        $zip->close();

        header("location:$url");

    } else {
        echo $open;
    }

} catch (Exception $e) {
    echo $e->getMessage();
}


?>
