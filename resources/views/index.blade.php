<!DOCTYPE html>

<html>

  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Sistema</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">

    <link rel="stylesheet" href="build/css/all.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
  </head>

  <body class="hold-transition skin-blue layout-top-nav" ng-app="app" ng-controller="GlobalController as globalCtrl">
    <div class="wrapper">
      <spinner></spinner>
      <ng-include src="'templates/menu.html'"></ng-include>

      <div id="main-content" class="content-wrapper" ui-view></div>

      <ng-include src="'templates/footer.html'"></ng-include>
    </div>
    <!-- ./wrapper -->

    <script src="build/js/vendors.js"></script>
    <script src="build/js/angular-with-plugins.js"></script>
    <script src="build/js/application.js"></script>
  </body>

</html>
