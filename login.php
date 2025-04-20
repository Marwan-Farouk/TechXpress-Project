<?php
session_start();
include_once('inc/connections.php');
if(isset$_POST['username']) && isset($_POST[password]){
    $username = stripcslashes(htmlstrotolowers($_POST['username']));
    $md5_pass = md5($_POST['password']);
    $username = filter_input(INPUT_POST,'username');
    //$password = stripcslashes(strtolower($_POST['password']));
    $username  = htmlentities(mysqli_real_escape_string($conn, $_POST['username']));
    $password  = htmlentities(mysqli_real_escape_string($conn, $_POST['password']));

if(isset($_POST['keep'])){
    $keep = htmlentities(mysqli_real_escape_string($conn,$_POST['keep']));
    if($keep ==1){
        setcookie('username',$username, time()+3600,"/"); 
        setcookie('password',$password, time()+3600,"/");
    }
}

}
if(empty($username)) {
    $usser_error = '<p id="error"> Plaease insert username <p>';
    $err_s = 1 ;
}

if(empty($password)) {
    $pass_error = '<p id="error"> Please insert Password <p>';
    $err_s = 1 ;
    include_once('index.php');
}
else{
    include('index.php');
}

if(!isset($err_s)){
    $sql = "SELECT id,username FROM users WHERE username='$username'AND 'md5_pass' ";
    $result = mysql_query($conn,$sql);
    $num_rows = mysqli_num_rows($result);
    if($num_rows !=0){
        $row = mysql_fetch_assoc($result);
        if ($row['username'] ===$username && $row['password'] === $password) {
            $_SESSION['username'] = $row['username'];
            $_SESSION['id'] =$row['id'];
            header('location: home.php');
            exit();
    }
   
    }else{
        $usser_error = '<p id="error"> worng username or password <p>';
        include_once('indix.php');
        exit();
    }
}

?>