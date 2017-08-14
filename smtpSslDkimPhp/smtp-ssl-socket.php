<?php
// more here 
// https://github.com/fxstar/Java/tree/master/CreateJavaSmtpServer/PhpSslSocketSmtp
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

// SSL socket
$ctx = stream_context_create();
stream_context_set_option($ctx, 'ssl', 'verify_peer', false);
stream_context_set_option($ctx, 'ssl', 'verify_peer_name', false);
try{
    echo $socket = stream_socket_client('ssl://127.0.0.1:587', $err, $errstr, 60, STREAM_CLIENT_CONNECT, $ctx);
    if (!$socket) {
        print "Failed to connect $err $errstr\n";
        return;
    }else{
        // Http
        // fwrite($socket, "GET / HTTP/1.0\r\nHost: www.example.com\r\nAccept: */*\r\n\r\n");
        // Smtp
        echo fread($socket,8192);
        echo fwrite($socket, "Hello localhost \n");
        echo fread($socket,8192);
        echo fwrite($socket, "QUIT \n");
        echo fread($socket,8192);
        fclose($socket);    
    }
}catch(Exception $e){
    echo $e;
}
die();
?>


<?php
/*
// read file contents
$filename = "bg.jpg";
$handle = fopen($filename, "rb");
$contents = fread($handle, filesize($filename));
fclose($handle);
*/
?>


<?php
try{
$socket = stream_socket_client("ssl://smtp.server.com:587", $errno, $errstr);
if ($socket) {		
	// fwrite($socket, "GET / HTTP/1.0\r\nHost: www.example.com\r\nAccept: */*\r\n\r\n");
	echo fwrite($socket, "Hello localhost");  
	echo fread($socket, 1024);
  echo fwrite($socket, "<QUIT>");
  echo fread($socket, 1024);
  /*
    echo fwrite($socket, "hello@breakermind.com<|>hello@breakermind.com<|>hello@breakermind.com");
   	echo fread($socket, 8192);
    echo fwrite($socket, "Temat wiadomości<|>Message<h1>Trochę html</h1><p>Footer wiadomości<p>");
   	echo fread($socket, 8192);
   	// filename
   	echo fwrite($socket, "bg.jpg");
   	echo fread($socket, 8192);
   	echo fwrite($socket, strlen($contents));   	
   	echo fwrite($socket, $contents);
   	echo fread($socket, 8192);
   	echo fwrite($socket, "<QUIT>");
    */
}else{
	echo "ERROR: $errno - $errstr\n";
}
}catch(Exception $e){
	echo $e;
}
?>
