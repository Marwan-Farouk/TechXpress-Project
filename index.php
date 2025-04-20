<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" constant="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>log in</title>
    <link rel="stylesheet' href="css/main.css">
</head>
<body>
<div class="main">

 <h1>Login</h1>  
 


<br>
<form action="login.php"method="POST">
    <?php if(isset($user_error)){
        echo $user_error;
    }
    ?>

<input type="text" name="username" id="username" placeholder="username" value="<?php if(isset($_COOKIE['username'])) echo$_COOKIE['username']; ?>"><dr>

<?php if(isset($pass_error)){
        echo $pass_error;
    }
    ?>

<input type="text" name="password" id="password" placeholder="password"value="<?php if(isset($_COOKIE['password'])) echo$_COOKIE['password']; ?>"><dr>

<label> <input type="checkbox" name="keep" id="keep" value="1" >keep me signed in </label>
<br>


<input type="submit" name="submit" id="submit" value="Log in"><dr>
</form>

<a href ="" > Forget password?</a>

<h3>Or</h3><br>

<a id="login" href="register.php">Register</a>


</div>
</body>
</html>