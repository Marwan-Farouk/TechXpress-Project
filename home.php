<?php
session_start();
include('inc/connections.php');
if (isset($_SESSION['id']) && isset($_SESSION['username'])) {
    $id = $_SESSION['id'];
    $user = $_SESSION['username'];
}
?>
<!DOCTYPE html>
<html>
<head>
    <title>Welcome Page</title>
</head>
<body>
    <a href="logout.php">Log out</a>
    <h1>Hello, <?php echo $user; ?></h1>
</body>
</html>

<?php