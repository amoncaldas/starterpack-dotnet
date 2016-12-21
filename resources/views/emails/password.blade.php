<!DOCTYPE html>
<html>
    <head>
        <title>Redefinição de senha</title>
        <link href="https://fonts.googleapis.com/css?family=Lato:100" rel="stylesheet" type="text/css">
    </head>
    <body>
        <div style="width: 100%; text-align:center; background-color:#fff; padding: 10px;">
            <img src="{{$message->embed(public_path().'/client/images/governo-do-estado-da-bahia.png')}}" alt="logo"
                style="max-width: 30%;">
            <hr>
            <div style="font-size: 20px;font-weight: bold;">
                Siga o passo abaixo para redefinir sua senha, caso não tenha sido você quem pediu
                a redefinição de senha favor desconsiderar este email.
            </div>
            <hr>
            <div>
                Clique ou copie o link para redefinir sua senha:<br>
                <a href="{{ url('/#/password/reset/'.$token) }}">{{ url('/#/password/reset/'.$token) }}</a>
            </div>
        </div>
    </body>
</html>
