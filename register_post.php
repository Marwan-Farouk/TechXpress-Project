<?php
include('inc/c0nnections.php');
if(isset($_POST['submit'])){
    $username = stripcslashes(strtolower($_POST['username']));
    $email = stripcslashes($_POST['email']);
    $password = stripcslashes($_POST['password']);
    if(isset($_POST['birthday_month']) && isset($_POST['birthday_day']) && isset($_POST['birthday_year'])){
        $birthday_month = (int)$_POST['birthday_month'];
        $birthday_day = (int)$_POST['birthday_day'];
        $birthday_year = (int)$_POST['birthday_year'];
        $birthday = htmlentities(mysqli_real_escape_string($conn,$birthday_day.'-'.$birthday_month.'-'.$birthday_year)); // " SELECT * FROM 'users' " 
    }


        $username  = htmlentities(mysqli_real_escape_string($conn, $_POST['username']));
        $email     = htmlentities(mysqli_real_escape_string($conn, $_POST['email']));
        $password  = htmlentities(mysqli_real_escape_string($conn, $_POST['password']));
        $md5_pass  = md5($password);

 if (isset($_POST['gender'])) {
    $gender = ($_POST['gender']);
    $gender = htmlentities(mysqli_real_escape_string($conn, $_POST['gender']));
    if (!in_array($gender, ['male', 'female'])) {
        $gender_error = '<p id="error"> Please choose gender not a text! <p>  <br>';
        $err_s = 1;
    }
}

$check_user = "SELECT * FROM 'user' WHERE username='$username' ";
$check_result = mysqli_query($conn_user);
$num_rows = mysqli_num_rows($check_result);
if($num_rows != 0){
    $user_error = '<p id="error">sorry username already exist , chnge another one <p> <br>';
}

if(empty($username)) {
    $usser_error = '<p id="error"> Plaease enter username <p>';
    $err_s = 1 ;
}elseif(strlen($username) <6 ){
    $user_error = ' Your username needs to have a minimum of 6 letters';
    $err_s = 1 ;

}elseif(filter_var($username,FILTER_VALIDATE_INT)){
    $user_error = '<p id="error"> Please enter a valid username not a number <p> ';
    $err_s = 1 ;
}

if(empty($email)) {
    $email_error = 'Please insert email <br>';
    $err_s = 1 ;
}
elseif(!filter_var($email,FILTER_VALIDATE_EMAIL))
{
    $email_error = ''<p id="error"> Please enter a valid email <p>';
    $err_s = 1 ;

}
if(empty($gender)) {
    $gender_error = '<p id="error"> Please choose gender not a text <p> ';
    $err_s = 1 ;
}
if(empty($birthday)){
    $birthday = '<p id="error">  Please insert date of birthday<p>';
    $err_s = 1 ;
    }
}
if(empty($password)) {
    $pass_error = '<p id="error"> Please insert Password <p>';
    $err_s = 1 ;
}elseif(strlen($password) <6) {
    $user_error =  '<p id="error"> Your password needs to have a minimum of 6 letters <p>';
    $err_s = 1 ;
    include('register.php');
}
else{
    if($err_s ==0) && ($num_rows == 0)){
        $sql = "INSERT INTO users(username,email,password,birthday,gender,md5_pass)
        VALUES ('$username','$email','$password','$birthday','$gender','$md5_pass')";
        mysql_query($conn,$sql);
        header('location:index.php');
    }else{
        include('register.php');
    }
}

}

?>